using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcCodeRouting;

namespace EmbeddedViews
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapCodeRoutes(
                baseRoute: MyPlugIn.Properties.Settings.Default.baseRoute,
              rootController: typeof(MyPlugIn.Controllers.HelloWorldController),
              settings: new CodeRoutingSettings
              {
                  EnableEmbeddedViews = true,
                  UseImplicitIdToken = true
              }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Test", action = "Index", id = UrlParameter.Optional }
            );

            
        }
    }
}