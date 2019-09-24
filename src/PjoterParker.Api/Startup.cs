using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PjoterParker.Api.Database;
using PjoterParker.Api.Filters;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Swagger;

namespace PjoterParker.Api
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            _configuration = configuration.Build();

            if (!hostingEnvironment.IsDevelopment())
            {
                _configuration = configuration.Build();
            }

            if (hostingEnvironment.IsDevelopment())
            {
                configuration.AddUserSecrets<Startup>();
                _configuration = configuration.Build();
            }

            _hostingEnvironment = hostingEnvironment;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc(routes => { });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pjoter Parker");
                c.DisplayRequestDuration();
            });
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
          services.AddMvc(configuration => { configuration.Filters.Add(typeof(ApiExceptionAttribute)); });
            services.AddDbContext<ApiDatabaseContext>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Pjoter Parker", Version = "v1" });
                c.MapType<Guid>(() => new Schema { Type = "string", Format = "text", Description = "GUID" });
                c.DescribeAllEnumsAsStrings();

                c.CustomSchemaIds(x =>
                {
                    int plusIndex = x.FullName.IndexOf("+");
                    int lastIndexOfDot = x.FullName.LastIndexOf(".");
                    int length = 0;

                    if (plusIndex != -1)
                    {
                        length = plusIndex - lastIndexOfDot - 1;
                    }
                    else
                    {
                        length = x.FullName.Length - lastIndexOfDot - 1;
                    }

                    return x.FullName.Substring(lastIndexOfDot + 1, length);
                });
            });

            var applicationContainer = IocConfig.RegisterDependencies(services, _hostingEnvironment, _configuration);
            var cache = applicationContainer.Resolve<IServer>();
            cache.FlushDatabase();

            return new AutofacServiceProvider(applicationContainer);
        }
    }
}