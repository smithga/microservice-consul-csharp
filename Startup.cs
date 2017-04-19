using System;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Owin;

namespace MicroservicesExample
{
    public class Startup
    {
        public static DateTime LastHealthCheck;
    
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            //config.Routes.MapHttpRoute(
            //    name: "Status",
            //    routeTemplate: "status",
            //    defaults: new { Controller = "Orders", Action = "Status" }
            //);
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "{Controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional, Controller = "Orders" }
            //);
            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();
            LastHealthCheck = DateTime.UtcNow;
            appBuilder.UseWebApi(config);
        }
    }

}
