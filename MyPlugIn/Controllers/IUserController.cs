using System;
namespace MyPlugIn.Controllers
{
    public interface IUserController
    {
        System.Web.Mvc.ActionResult Create();
        System.Web.Mvc.ActionResult Create(MyPlugIn.Models.UserViewModel userviewmodel);
        System.Web.Mvc.ActionResult Delete(int id = 0);
        System.Web.Mvc.ActionResult DeleteConfirmed(int id);
        System.Web.Mvc.ActionResult Details(int id = 0);
        System.Web.Mvc.ActionResult Edit(MyPlugIn.Models.UserViewModel userviewmodel);
        System.Web.Mvc.ActionResult Edit(int id = 0);
        System.Web.Mvc.ActionResult Index();
    }
}
