using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAPI.Identity.Configuration
{
    public static class IdentityServerConfiguration
    {
        private static readonly string DisplayName = "Maxtrix Demo API";

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResource("TaskAPI", new List<string> {
                    CustomClaimTypes.UserId,
                    CustomClaimTypes.UserEmail,
                    CustomClaimTypes.UserName,
                    CustomClaimTypes.UserSurname
                })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources(string scope)
        {
            return new List<ApiResource>
            {
                new ApiResource(scope, DisplayName)
            };
        }

        public static IEnumerable<Client> GetClients(string clientId, string clientSecret)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = clientId,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 60 * 60 * 24,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    ClientSecrets = new List<Secret> {
                        new Secret(clientSecret.Sha256())
                    },
                    AllowedScopes = { "api1", IdentityServerConstants.StandardScopes.OfflineAccess }
                }
            };
        }

    }
}
