﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace VideoForum.Utility
{
    public static class SD
    {
        public static string IsActive( this IHtmlHelper html, string controller, string action, string cssClass = "active" )
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