using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PjoterParker.Auth.Database;
using PjoterParker.Entities;

namespace PjoterParker.Auth
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
                //configuration.Add(new AzureSecretsVaultSource(_configuration["AzureKeyVault:App:BaseUrl"], _configuration["AzureKeyVault:App:ClientId"], _configuration["AzureKeyVault:App:SecretId"]));
                _configuration = configuration.Build();
            }

            if (hostingEnvironment.IsDevelopment())
            {
                configuration.AddUserSecrets<Startup>();
                _configuration = configuration.Build();
            }

            _hostingEnvironment = hostingEnvironment;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AuthDatabaseContext context)
        {
            app.UseIdentityServer();
            app.UseMvc(routes => { });
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<AuthDatabaseContext>();
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuthDatabaseContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                //.AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddInMemoryApiResources(Config.GetApiResources())
                //.AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<User>();

            var applicationContainer = IocConfig.RegisterDependencies(services, _hostingEnvironment, _configuration);
            return new AutofacServiceProvider(applicationContainer);
        }
    }
}