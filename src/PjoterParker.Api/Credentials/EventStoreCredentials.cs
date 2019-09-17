using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PjoterParker.Common.Credentials;
using PjoterParker.Core.Credentials;

namespace PjoterParker.Api.Credentials
{
    public class EventStoreCredentials : IConnectionString
    {
        public EventStoreCredentials(EventStoreCredentialsBase credentials)
        {
            ConnectionString = credentials.ConnectionString;
        }

        public string ConnectionString { get; private set; }
    }
}
