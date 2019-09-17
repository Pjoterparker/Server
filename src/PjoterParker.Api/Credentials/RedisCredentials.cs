using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PjoterParker.Common.Credentials;

namespace PjoterParker.Api.Credentials
{
    public class RedisCredentials : RedisCredentialsBase
    {
        public RedisCredentials(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
