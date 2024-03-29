﻿using PjoterParker.Common.Credentials;
using PjoterParker.Core.Credentials;

namespace PjoterParker.Api.Credentials
{
    public class RedisCredentials : IConnectionString
    {
        public RedisCredentials(RedisLocalCredentialsBase credentials)
        {
            ConnectionString = credentials.ConnectionString;
        }

        public RedisCredentials(RedisAzureCredentialsBase credentials)
        {
            ConnectionString = credentials.ConnectionString;
        }

        public string ConnectionString { get; private set; }
    }
}