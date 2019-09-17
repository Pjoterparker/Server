using Microsoft.Extensions.Configuration;
using PjoterParker.Core.Credentials;
using PjoterParker.Core.Extensions;

namespace PjoterParker.Common.Credentials
{
    public abstract class RedisCredentialsBase : IConnectionString
    {
        private readonly string _name;

        private readonly string _password;

        private readonly int _port;

        private readonly bool _isSsl;

        private readonly bool _allowAdmin;

        protected RedisCredentialsBase(IConfiguration configuration)
        {
            _name = configuration["Redis:Name"];
            _password = configuration["Redis:Password"];
            _port = configuration["Redis:Port"].To<int>();

            _isSsl = true;
            _allowAdmin = true;
        }

        public string ConnectionString => $"{_name}.redis.cache.windows.net:{_port},password={_password},ssl={_isSsl},abortConnect=False,allowAdmin={_allowAdmin}";
    }
}