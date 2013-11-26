using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPlugIn.Extensions
{
    public static class MyExtensions
    {
        public static string Content(this UrlHelper helper, string contentPath, bool includePlugInRoute = true)
        { 
            var routeInsertIndex = contentPath.IndexOf('/');
            return helper.Content(contentPath.Insert(routeInsertIndex, string.Format("/{0}", Properties.Settings.Default.baseRoute))); 
        }

        public static void AddViewLocationFormat(this RazorViewEngine engine, string path)
        {
            List<string> existingPaths = new List<string>(engine.ViewLocationFormats);
            existingPaths.Add(path);

            engine.ViewLocationFormats = existingPaths.ToArray();
        }

        public static void AddPartialViewLocationFormat(this RazorViewEngine engine, string path)
        {
            List<string> existingPaths = new List<string>(engine.PartialViewLocationFormats);
            existingPaths.Add(path);

            engine.PartialViewLocationFormats = existingPaths.ToArray();
        }
    }
}