using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MvcCodeRouting;
using WebActivatorEx;
using MyPlugin;
using MyPlugIn.Extensions;
using Microsoft.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AppStart), "Start")]
namespace MyPlugin
{

    public static class AppStart
    {
        public static void Start()
        {
            ConfigureRoutes();
            AddCustomViewLocations();
            ViewEngines.Engines.EnableCodeRouting();
        }

        private static void AddCustomViewLocations()
        {
            var razorEngine = ViewEngines.Engines.First(x => x.GetType().Equals(typeof(FixedRazorViewEngine))) as FixedRazorViewEngine;
            razorEngine.AddViewLocationFormat(string.Format("~/Views/{0}/{{1}}/{{0}}.cshtml", MyPlugIn.Properties.Settings.Default.baseRoute));
            razorEngine.AddPartialViewLocationFormat(string.Format("~/Views/{0}/{{1}}/{{0}}.cshtml", MyPlugIn.Properties.Settings.Default.baseRoute));
            razorEngine.AddPartialViewLocationFormat(string.Format("~/Views/{0}/Shared/{{0}}.cshtml", MyPlugIn.Properties.Settings.Default.baseRoute));
        }

        private static void ConfigureRoutes()
        {
            RouteTable.Routes.MapCodeRoutes(
                baseRoute: MyPlugIn.Properties.Settings.Default.baseRoute,
              rootController: typeof(MyPlugIn.Controllers.HelloWorldController),
              settings: new CodeRoutingSettings
              {
                  EnableEmbeddedViews = true,
                  UseImplicitIdToken = true
              }
           );
        }


    }
}