using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using MvcCodeRouting;
using System.Net;
using MyPlugIn.Extensions;


namespace MyPlugIn.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly static object _lockObject = new object();

        private static IEnumerable<string> _manifestResourceNames = null;

        //
        // GET: /Resources/
        [OutputCache(VaryByParam="*", Duration=3600)]
        [CustomRoute("{*resourcePath}")]
        public ActionResult Index([FromRoute]string resourcePath)
        {
            var pathParts = resourcePath.Split('/');
            var embeddedResourcePath = string.Join(".", pathParts);
            var resourceName = pathParts.Last();

            embeddedResourcePath = string.Format("MyPlugIn.Resources.{0}", embeddedResourcePath);

            lock (_lockObject)
            {
                if (_manifestResourceNames == null)
                    _manifestResourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            }

            embeddedResourcePath = _manifestResourceNames.FirstOrDefault(x => x.Equals(embeddedResourcePath, StringComparison.InvariantCultureIgnoreCase));

            if(embeddedResourcePath == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourcePath);

            return new FileStreamResult(resourceStream, resourceName.GetMimeType());
        }

    }
}
