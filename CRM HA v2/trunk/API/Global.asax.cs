using MobileServices.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
//using System.Web.Optimization;
using System.Web.Routing;

namespace MobileServices
{
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
         //   AreaRegistration.RegisterAllAreas();

            //GlobalConfiguration.Configuration.MapHttpAttributeRoutes();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register); //llamo a este para q no falle el MapHttpAttributeRoutes
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Verb Routing 
       
            GlobalConfiguration
                .Configuration
                .Formatters
                .Insert(0, new JsonpFormatter());
            //RouteTable.Routes.MapHttpRoute(
            //    name: "ActionApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
           
          //  RouteTable.Routes.MapHttpRoute(name: "ApiEventos", routeTemplate: "api/{controller}/{id}/{stimeStamp}");
           RouteTable.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });
           

            
            

            //GlobalConfiguration.Configuration.MessageHandlers.Add(new APILoggerHandler());
            //bool TESTING = true;
            //if (TESTING)
            //    GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsHandler());

        }
    }
}