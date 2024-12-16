using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace VideoForum.Extensions
{
    /// <summary>
    /// A static class with several extension methods of ClaimsPrincipal for getting the values of the claims inside the cookies
    /// </summary>
    public static class UserClaimsExtension
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static string GetName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.GivenName)?.Value;
        }

        //The Id needs to be reconverted to an integer because the claim constructor only takes string values
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
