using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Common.Identity
{
    public static class IdentityExtensions
    {
        public static Guid GetUserId(this IHttpContextAccessor context)
        {
            return new Guid(context.HttpContext.User.FindFirst(CustomClaimTypes.UserId).Value);
        }

        public static string GetUserEmail(this IHttpContextAccessor context)
        {
            return context.HttpContext.User.FindFirst(CustomClaimTypes.UserEmail).Value;
        }

        public static string GetUserName(this IHttpContextAccessor context)
        {
            return context.HttpContext.User.FindFirst(CustomClaimTypes.UserName).Value;
        }

        public static string GetUserSurname(this IHttpContextAccessor context)
        {
            return context.HttpContext.User.FindFirst(CustomClaimTypes.UserSurname).Value;
        }
    }
}
