using MyPlugIn.Models;
using MyPlugIn.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPlugIn.Controllers
{
    public class PeoplePickerController : Controller
    {
        //
        // GET: /PeoplePicker/

        public ActionResult Search(string search)
        {
            if (string.IsNullOrEmpty(search) || search.Length < 3)
                return Content("Please provide at least 3 letters in your search term", "text/plain");

            return PartialView("PeoplePickerSearchResult", ActiveDirectoryHelper.FindUsers(search).OrderBy(x => x.LastName));
        }

    }
}
