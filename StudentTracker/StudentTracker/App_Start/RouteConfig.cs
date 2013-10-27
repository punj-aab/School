using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StudentTracker
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "CheckUser",
               url: "Organization/CheckUser/{id}",
               defaults: new { controller = "Organization", action = "CheckUser", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "index", id = UrlParameter.Optional }
            );
        }
    }
}