using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PjoterParker.Core.Services;
using Serilog;

namespace PjoterParker.Api.Filters
{
    public sealed class ApiExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly IGuidService _guid;

        private readonly IHostingEnvironment _hosting;

        private readonly ILogger _logger;

        public ApiExceptionAttribute(ILogger logger, IHostingEnvironment hosting, IGuidService guid)
        {
            _logger = logger;
            _hosting = hosting;
            _guid = guid;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.InnerException is ValidationException exceptionInner)
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(exceptionInner.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(e => e.Key, e => e.Select(element => element.ErrorMessage).ToList()));
                return;
            }

            if (context.Exception is ValidationException exception)
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new JsonResult(exception.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(e => e.Key, e => e.Select(element => element.ErrorMessage).ToList()));
                return;
            }

            var exceptionId = _guid.New();

            context.HttpContext.Response.StatusCode = 500;
            var sb = new StringBuilder();
            sb.AppendLine("ErrorId: " + exceptionId);
            sb.AppendLine(context.HttpContext.Request.GetDisplayUrl());

            if (context.HttpContext.Request.Body.CanSeek)
            {
                context.HttpContext.Request.Body.Position = 0;
                using (var reader = new StreamReader(context.HttpContext.Request.Body))
                {
                    sb.AppendLine(reader.ReadToEnd());
                }
            }

            sb.AppendLine();
            sb.Append(context.Exception);

            _logger.Error(sb.ToString());

            if (_hosting.IsDevelopment())
            {
                context.Result = new ContentResult() { Content = sb.ToString() };
            }
            else
            {
                context.Result = new ContentResult() { Content = exceptionId.ToString() };
            }
        }
    }
}
