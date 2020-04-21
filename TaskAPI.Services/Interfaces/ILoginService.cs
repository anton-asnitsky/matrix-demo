using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Services.Models.Inbound;
using TokenResponse = IdentityModel.Client.TokenResponse;

namespace TaskAPI.Services.Interfaces
{
    public interface ILoginService
    {
        Task<TokenResponse> Login(LoginRequest request);
        Task RecoverPassword(RecoverPasswordRequest request);
        Task PasswordChange(PasswordChangeRequest request);
        Task<(string accessToken, string refreshToken)> GetToken(string refreshToken);
    }
}
