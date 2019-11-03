using Microsoft.Extensions.Configuration;

namespace PjoterParker.Common.Credentials
{
    public class AzureActiveDirectoryCredentials
    {
        public AzureActiveDirectoryCredentials(IConfiguration configuration, string tenantName)
        {
            ClientId = configuration[$"AzureActiveDirectory:{tenantName}:ClientId"];
            ClientSecret = configuration[$"AzureActiveDirectory:{tenantName}:ClientSecret"];
            TenantId = configuration[$"AzureActiveDirectory:{tenantName}:TenantId"];
            TenantName = configuration[$"AzureActiveDirectory:{tenantName}:TenantName"];
        }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string TenantId { get; set; }

        public string TenantName { get; set; }
    }
}