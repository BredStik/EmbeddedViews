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
    }
}