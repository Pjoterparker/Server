﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using EventStore.ClientAPI;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PjoterParker.Api.Credentials;
using PjoterParker.Api.Database;
using PjoterParker.Common.Commands;
using PjoterParker.Common.Credentials;
using PjoterParker.Common.Events;
using PjoterParker.Common.Services;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Events;
using PjoterParker.Core.Specification;
using PjoterParker.Core.Validation;
using PjoterParker.Development;
using PjoterParker.Domain.Locations;
using PjoterParker.EventStore;
using Serilog;
using StackExchange.Redis;

namespace PjoterParker.Api
{
    public class IocConfig
    {
        public static void OverrideWithLocalCredentials(ContainerBuilder builder)
        {
            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                return new ApiDatabaseCredentials(new SqlServerDatabaseCredentials(configuration, "Api"));
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var settings = ConnectionSettings.Create();
                settings.UseConsoleLogger();
                settings.SetHeartbeatTimeout(TimeSpan.FromMinutes(1));
                settings.KeepReconnecting();
                settings.KeepRetrying();

                return settings.Build();
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                return new RedisCredentials(new RedisLocalCredentialsBase(configuration));
            }).SingleInstance();
        }

        public static void RegisterCredentials(ContainerBuilder builder)
        {
            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                return new EventStoreCredentials(new EventStoreCredentialsBase(configuration));
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                return new ApiDatabaseCredentials(new SqlServerDatabaseCredentials(configuration, "Api"));
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                return new RedisCredentials(new RedisAzureCredentialsBase(configuration));
            }).SingleInstance();
        }

        public static IContainer RegisterDependencies(IServiceCollection services, IHostingEnvironment env, IConfiguration rootConfiguration)
        {
            var domainAssembly = typeof(CreateLocation).GetTypeInfo().Assembly;

            var builder = new ContainerBuilder();
            builder.Populate(services);
            RegisterCredentials(builder);

            if (env.IsDevelopment())
            {
                OverrideWithLocalCredentials(builder);
            }

            builder.Register(b => rootConfiguration).SingleInstance();
            builder.Register<Serilog.ILogger>(b =>
            {
                var configuration = b.Resolve<IConfiguration>();

                const string format = "{NewLine}{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {NewLine}{Message:lj}{NewLine}{Exception}";

                return new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console(outputTemplate: format)
                    .WriteTo.Seq(configuration["Seq:Url"])
                    .CreateLogger();
            }).SingleInstance();

            builder.Register(b =>
            {
                var credentials = b.Resolve<RedisCredentials>();
                return ConnectionMultiplexer.Connect(credentials.ConnectionString);
            }).SingleInstance();

            builder.Register(b =>
            {
                var redis = b.Resolve<ConnectionMultiplexer>();
                return redis.GetDatabase();
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();
                var redis = b.Resolve<ConnectionMultiplexer>();
                return redis.GetServer(host: configuration["Redis:Url"], port: 6379);
            }).SingleInstance();

            builder.Register(b =>
            {
                var settings = b.Resolve<ConnectionSettings>();

                var credentials = b.Resolve<EventStoreCredentials>();
                var connection = EventStoreConnection.Create(settings, new Uri(credentials.ConnectionString));

                var task = connection.ConnectAsync();
                task.Wait();

                return connection;
            }).SingleInstance();

            builder.Register(b =>
            {
                var configuration = b.Resolve<IConfiguration>();

                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri(configuration["RabbitMq:Url"]), h =>
                    {
                        h.Username(configuration["RabbitMq:Username"]);
                        h.Password(configuration["RabbitMq:Password"]);
                    });

                    cfg.UseSerilog();

                    cfg.ReceiveEndpoint(host, "commands", ep =>
                    {
                        ep.Handler<CreateLocation>(context =>
                        {
                            return Console.Out.WriteLineAsync($"Received: {context.CorrelationId}");
                        });
                    });
                });

                return busControl;
            })
            .SingleInstance().As<IBusControl>().As<IBus>();

            //builder.Register<IProjectionsManager>(b =>
            //{
            //    var configuration = b.Resolve<IConfiguration>();
            //    var credentials = b.Resolve<EventStoreCredentials>();
            //    var manager = new ProjectionsManager(new ConsoleLogger(), new IPEndPoint(IPAddress.Parse(credentials.Ip), 2113), TimeSpan.FromSeconds(10));
            //    return new AppProjectionsManager(manager, credentials);
            //}).SingleInstance();

            builder.RegisterType<EventDispatcher>().As<IEventDispatcher>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(IApply<>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(IAggregateMap<>)).InstancePerLifetimeScope();

            builder.RegisterType<Rabbitmq.CommandDispatcher>().As<ICommandDispatcher>().InstancePerLifetimeScope();
            builder.RegisterType<CommandFactory>().As<ICommandFactory>().InstancePerLifetimeScope();
            builder.RegisterType<EventFactory>().As<IEventFactory>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(ICommandHandlerAsync<>)).InstancePerLifetimeScope();

            //builder.RegisterType<QueryDispatcher>().As<IQueryDispatcher>().InstancePerRequest().InstancePerLifetimeScope();
            //builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IQueryHandler<,>)).InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(AppAbstractValidation<>)).InstancePerLifetimeScope();

            builder.RegisterType<ApiDatabaseContext>().As<IApiDatabaseContext>().InstancePerLifetimeScope();

            builder.RegisterType<GuidService>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<EventStoreAggregateStore>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(ISpecificationFor<,>)).InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(domainAssembly)
            .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
            .As<Profile>().SingleInstance();

            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}