using System.IO;
using System.Linq;
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

            context.HttpContext.Response.StatusCode = 500;

            string body = string.Empty;
            if (context.HttpContext.Request.Body.CanSeek)
            {
                context.HttpContext.Request.Body.Position = 0;
                using (var reader = new StreamReader(context.HttpContext.Request.Body))
                {
                    body = reader.ReadToEnd();
                }
            }

            var exceptionId = _guid.New();
            _logger.Error("ExceptionId: {exceptionId} Url: {url} Body: {body} Exception: {exception}", exceptionId, context.HttpContext.Request.GetDisplayUrl(), body, context.Exception);

            if (_hosting.IsDevelopment())
            {
                context.Result = new ContentResult() { Content = context.Exception.ToString() };
            }
            else
            {
                context.Result = new ContentResult() { Content = exceptionId.ToString() };
            }
        }
    }
}