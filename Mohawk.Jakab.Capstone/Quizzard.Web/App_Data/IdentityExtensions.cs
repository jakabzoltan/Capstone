using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Mohawk.Jakab.Quizzard.Services.Handlers;

namespace Quizzard.Web.App_Data
{
    public static class IdentityExtensions
    {
        public static string GetName(this IPrincipal principal)
        {
            if (principal?.Identity?.GetUserId() == null) return "";
            var userService = new UserService();
            return userService.GetFullName(principal.Identity.GetUserId());
        }

        public static bool IsVerified(this IPrincipal principal)
        {
            if (principal?.Identity?.GetUserId() == null) return false;
            var userService = new UserService();
            return userService.UserIsVerified(principal.Identity.GetUserId());
        }

    }
}