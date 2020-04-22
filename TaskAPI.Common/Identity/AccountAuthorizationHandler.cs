using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI.Common.Identity
{
    public class AccountAuthorizationHandler : AuthorizationHandler<AccountAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AccountAuthorizationRequirement requirement)
        {
            var hasClaims = context.User.HasClaim(c => c.Type == CustomClaimTypes.UserId) &&
                            context.User.HasClaim(c => c.Type == CustomClaimTypes.UserEmail);
            if (hasClaims)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
