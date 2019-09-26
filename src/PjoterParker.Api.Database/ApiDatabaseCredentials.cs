using PjoterParker.Common.Credentials;
using PjoterParker.Core.Credentials;

namespace PjoterParker.Api.Database
{
    public class ApiDatabaseCredentials : IConnectionString
    {
        public ApiDatabaseCredentials(LocalDatabaseCredentials credentials)
        {
            ConnectionString = credentials.ConnectionString;
        }

        public ApiDatabaseCredentials(SqlServerDatabaseCredentials credentials)
        {
            ConnectionString = credentials.ConnectionString;
        }

        public string ConnectionString { get; private set; }
    }
}
