using IdentityModel.Client;
using IdentityServer4;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Common.Exceptions;
using TaskAPI.Common.Options;
using TaskAPI.Services.Interfaces;
using TaskAPI.Services.Models.Inbound;
using TokenResponse = IdentityModel.Client.TokenResponse;

namespace TaskAPI.Services
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IdentityServerOptions _identityServerOptions;

        public IdentityProvider(
            IHttpClientFactory httpClientFactory, 
            IOptions<IdentityServerOptions> options
        ) {
            _httpClientFactory = httpClientFactory;
            _identityServerOptions = options.Value;
        }

        private async Task<(HttpClient client, string endpoint)> SetupClient()
        {
            var client = _httpClientFactory.CreateClient();
            var discovery = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _identityServerOptions.Host,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false
                }
            });

            if (discovery.IsError)
                throw new Exception(discovery.Error);

            return (client, discovery.TokenEndpoint);
        }

        public async Task<TokenResponse> RequestPasswordTokenAsync(LoginRequest request)
        {
            var (client, endpoint) = await SetupClient();

            var token = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = endpoint,
                ClientId = _identityServerOptions.ClientId,
                ClientSecret = _identityServerOptions.ClientSecret,
                Scope = $"{_identityServerOptions.Scope} {IdentityServerConstants.StandardScopes.OfflineAccess}",

                UserName = request.Email,
                Password = request.Password
            });
            return token;
        }

        public async Task<(string accessToken, string refreshToken)> RequestRefreshToken(string refreshToken)
        {
            var (client, endpoint) = await SetupClient();

            var token = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = endpoint,
                ClientId = _identityServerOptions.ClientId,
                ClientSecret = _identityServerOptions.ClientSecret,
                Scope = _identityServerOptions.Scope,
                RefreshToken = refreshToken
            });

            if (token.IsError)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "Authentication failed");
            }

            return (accessToken: token.AccessToken, refreshToken: token.RefreshToken);
        }
    }
}
