using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskAPI.Common.Identity;
using TaskAPI.Identity.Configuration;

namespace TaskAPI.Identity.Providers
{
    public class CustomProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = context.Subject.Claims.ToList();
            var userId = claims.Find(c => c.Type == CustomClaimTypes.UserId).Value;
            var userEmail = claims.Find(c => c.Type == CustomClaimTypes.UserEmail).Value;
            var userName = claims.Find(c => c.Type == CustomClaimTypes.UserName).Value;
            var userSurname = claims.Find(c => c.Type == CustomClaimTypes.UserSurname).Value;

            context.IssuedClaims = new List<Claim>
            {
                new Claim(CustomClaimTypes.UserId, userId),
                new Claim(CustomClaimTypes.UserEmail, userEmail),
                new Claim(CustomClaimTypes.UserName, userName),
                new Claim(CustomClaimTypes.UserSurname, userSurname)
            };

            return Task.FromResult(true);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            /** TODO: implement if needed */
            return Task.FromResult(true);
        }
    }
}
