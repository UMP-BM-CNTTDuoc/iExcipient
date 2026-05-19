using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace iExcipient_API
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "HopeDetails",
                url: "hope/detail/{id}",
                defaults: new { controller = "Hope", action = "Detail", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "HopeSearch",
                url: "hope",
                defaults: new { controller = "Hope", action = "Index" }
            );

            routes.MapRoute(
                name: "CosingDetails",
                url: "cosing/detail/{id}",
                defaults: new { controller = "Cosing", action = "Detail", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CosingSearch",
                url: "cosing",
                defaults: new { controller = "Cosing", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}