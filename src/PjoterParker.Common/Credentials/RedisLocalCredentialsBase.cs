using Microsoft.Extensions.Configuration;
using PjoterParker.Common.Extensions;
using PjoterParker.Core.Credentials;

namespace PjoterParker.Common.Credentials
{
    public sealed class RedisLocalCredentialsBase : IConnectionString
    {
        private readonly bool _allowAdmin;

        private readonly bool _isSsl;

        private readonly string _ip;

        private readonly int _port;

        public RedisLocalCredentialsBase(IConfiguration configuration)
        {
            _ip = configuration["Redis:Ip"];
            _port = configuration["Redis:Port"].To<int>();

            _isSsl = true;
            _allowAdmin = true;
        }

        public string ConnectionString => $"{_ip}:{_port},ssl={_isSsl},abortConnect=False,allowAdmin={_allowAdmin}";
    }
}