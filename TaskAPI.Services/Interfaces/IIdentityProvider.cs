using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Services.Models.Inbound;
using TokenResponse = IdentityModel.Client.TokenResponse;

namespace TaskAPI.Services.Interfaces
{
    public interface IIdentityProvider
    {
        Task<TokenResponse> RequestPasswordTokenAsync(LoginRequest request);
        Task<(string accessToken, string refreshToken)> RequestRefreshToken(string refreshToken);
    }
}
