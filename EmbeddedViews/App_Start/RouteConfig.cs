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

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapCodeRoutes(
                baseRoute: "hello",
              rootController: typeof(MyPlugIn.Controllers.HelloController),
              settings: new CodeRoutingSettings
              {
                  EnableEmbeddedViews = true,
                  UseImplicitIdToken = true
              }
           );
        }
    }
}