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