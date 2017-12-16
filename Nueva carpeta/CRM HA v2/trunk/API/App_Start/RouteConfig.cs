using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MobileServices
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("{resource}.axd/{*pathInfo}");

            //routes.MapPageRoute(
            //     "Default",
            //     "{controller}/{action}/{id}"
            //     //,
            //     //new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}