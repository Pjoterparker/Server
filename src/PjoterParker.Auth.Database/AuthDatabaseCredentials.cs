using PjoterParker.Common.Credentials;
using PjoterParker.Core.Credentials;

namespace PjoterParker.Auth.Database
{
    public class AuthDatabaseCredentials : IConnectionString
    {
        public AuthDatabaseCredentials(LocalDatabaseCredentials credentials)
        {
            ConnectionString = credentials.ConnectionString;
        }

        public AuthDatabaseCredentials(SqlServerDatabaseCredentials credentials)
        {
            ConnectionString = credentials.ConnectionString;
        }

        public string ConnectionString { get; private set; }
    }
}