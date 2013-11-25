using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmbeddedViews.Controllers
{
    public class testController : Controller
    {
        //
        // GET: /test/

        public ActionResult Index()
        {
            return View();//"~/Views/framework/test/index.cshtml"
        }

        public ActionResult Edit()
        {
            return View("index");
        }

    }
}
