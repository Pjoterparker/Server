﻿using Microsoft.Extensions.Configuration;
using PjoterParker.Core.Credentials;
using PjoterParker.Core.Extensions;

namespace PjoterParker.Common.Credentials
{
    public class EventStoreCredentials : IConnectionString
    {
        public EventStoreCredentials(IConfiguration configuration)
        {
            User = configuration["EventStore:User"];
            Password = configuration["EventStore:Password"];
            Ip = configuration["EventStore:Ip"];
            Port = configuration["EventStore:Port"].To<int>();
        }

        public string Ip { get; }

        public string Password { get; }

        public int Port { get; }

        public string User { get; }

        public string ConnectionString => $"tcp://{User}:{Password}@{Ip}:{Port}";
    }
}