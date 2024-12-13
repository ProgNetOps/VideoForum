using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoForum.Utility
{
    public static class SD
    {
        public const string AdminRole = "admin";
        public const string ModeratorRole = "moderator";
        public const string UserRole = "user";

        public static readonly List<string> Roles = [AdminRole, ModeratorRole, UserRole];

        public static string IsActive(this IHtmlHelper html, string controller, string action, string cssClass = "active")
        {
            var routeData = html.ViewContext.RouteData;
            var routeAction = routeData.Values["action"]?.ToString();
            var routeController = routeData.Values["controller"]?.ToString();

            return controller == routeController && action == routeAction ?
                cssClass :
                string.Empty;
        }
    }
}
