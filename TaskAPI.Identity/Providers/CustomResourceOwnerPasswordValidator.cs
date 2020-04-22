using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskAPI.Common;
using TaskAPI.Common.Identity;
using TaskAPI.Data.DataContexts;
using TaskAPI.Data.Models;
using TaskAPI.Identity.Configuration;

namespace TaskAPI.Identity.Providers
{
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly DataContext _dataContext;

        public CustomResourceOwnerPasswordValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context) {
            var user = _dataContext.Users.Where(u => u.Email == context.UserName).SingleOrDefault();

            if (user != null) {
                if (Utils.VerifyPassword(user.Password, context.Password))
                {
                    context.Result = GetGrantValidationResult(user);
                }
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid credentials");
            }

            await Task.FromResult(0);
        }

        private static GrantValidationResult GetGrantValidationResult(User user)
        {
            return new GrantValidationResult(
                user.JwtToken,
                OidcConstants.AuthenticationMethods.Password,
                claims: new List<Claim>
                {
                    new Claim(CustomClaimTypes.UserId, user.UserId.ToString()),
                    new Claim(CustomClaimTypes.UserEmail, user.Email),
                    new Claim(CustomClaimTypes.UserName, user.FirstName),
                    new Claim(CustomClaimTypes.UserSurname, user.LastName)
                }
            );
        }
    }
}
