using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PjoterParker.Auth.Database;
using PjoterParker.Common.Credentials;
using Serilog;
using Serilog.Events;

namespace PjoterParker.Auth
{
    public class IocConfig
    {
        public static void OverrideWithLocalCredentials(ContainerBuilder builder)
        {
            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                return new AuthDatabaseCredentials(new LocalDatabaseCredentials(configuration, "Auth"));
            }).SingleInstance();
        }

        public static void RegisterCredentials(ContainerBuilder builder)
        {
            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                return new AzureActiveDirectoryCredentials(configuration, "Softserve");
            }).SingleInstance();
        }

        public static IContainer RegisterDependencies(IServiceCollection services, IHostingEnvironment env, IConfiguration rootConfiguration)
        {
            Assembly assemblies = typeof(Program).Assembly;

            var builder = new ContainerBuilder();
            builder.Populate(services);
            RegisterCredentials(builder);

            if (env.IsDevelopment())
            {
                OverrideWithLocalCredentials(builder);
            }

            builder.Register(b => rootConfiguration).SingleInstance();

            builder.Register<ILogger>(b =>
            {
                const string format = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {NewLine}{Message:lj}{NewLine}{Exception}";

                return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: format)
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error, outputTemplate: format)
                .WriteTo.Logger(cl => cl.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Information).WriteTo.File("Logs/queries.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: format))
                .CreateLogger();
            });

            return builder.Build();
        }
    }
}