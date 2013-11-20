using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using MvcCodeRouting;


namespace MyPlugIn.Controllers
{
    public class ImagesController : Controller
    {
        //
        // GET: /Images/

        
        [OutputCache(VaryByParam="*", Duration=3600)]
        [CustomRoute("{imageName}")]
        public ActionResult Index([FromRoute]string imageName)
        {
            var imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("MyPlugIn.Images.{0}", imageName));
            return new FileStreamResult(imageStream, "image/gif");
        }

    }
}
