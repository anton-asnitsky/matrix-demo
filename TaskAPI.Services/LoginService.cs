using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Common;
using TaskAPI.Common.Exceptions;
using TaskAPI.Data.DataContexts;
using TaskAPI.Services.Interfaces;
using TaskAPI.Services.Models.Inbound;
using TaskAPI.Services.Models.Outbound;

namespace TaskAPI.Services
{
    public class LoginService : ILoginService
    {
        private readonly IMailer _mailer;
        private readonly IIdentityProvider _identityProvider;
        private readonly IRequestValidator _requestValidator;
        private readonly DataContext _dataContext;


        public LoginService(
            IMailer mailer,
            IIdentityProvider identityProvider,
            IRequestValidator requestValidator,
            DataContext dataContext
        ) {
            _mailer = mailer;
            _identityProvider = identityProvider;
            _requestValidator = requestValidator;
            _dataContext = dataContext;
        }

        public async Task<(string accessToken, string refreshToken)> GetToken(string refreshToken)
        {
            return await _identityProvider.RequestRefreshToken(refreshToken);
        }

        public async Task<TokenResponse> Login(LoginRequest request)
        {
            var result = _requestValidator.Validate(request);
            if (!result.IsValid)
            {
                throw new InvalidValueException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToArray());
            }

            var token = await _identityProvider.RequestPasswordTokenAsync(request);
            if (token.IsError)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "Authentication failed");
            }

            return JsonConvert.DeserializeObject<TokenResponse>(token.Raw);
        }

        public async Task PasswordChange(PasswordChangeRequest request)
        {
            var result = _requestValidator.Validate(request);
            if (!result.IsValid)
            {
                throw new InvalidValueException(result.Errors.Select(e => e.ErrorMessage).ToArray());
            }

            var user = await _dataContext.Users.Where(u => u.PasswordRecoveryToken == request.Token).SingleOrDefaultAsync();

            if (user == null)
                throw new AccessException("Password recovery session is invalid.");

            user.Password = Utils.PasswordToHash(request.Password);
            user.PasswordRecoveryToken = "";
            await _dataContext.SaveChangesAsync();
            
            await _mailer.SendPasswordChanged(user.Email, $"{user.FirstName} {user.LastName}");
        }

        public async Task RecoverPassword(RecoverPasswordRequest request)
        {
            var result = _requestValidator.Validate(request);
            if (!result.IsValid)
            {
                throw new InvalidValueException(result.Errors.Select(e => e.ErrorMessage).ToArray());
            }

            var user = await _dataContext.Users.Where(u => u.Email == request.Email).SingleOrDefaultAsync();

            if (user == null)
            {
                throw new InvalidValueException(new[] { "Incorrect email." });
            }

            var token = Guid.NewGuid().GuidEncode();
            user.PasswordRecoveryToken = token;
            await _dataContext.SaveChangesAsync();

            await _mailer.SendRecoverPassword(user.Email, $"{user.FirstName} {user.LastName}", token);
        }
    }
}
