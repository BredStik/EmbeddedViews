using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPlugIn.Models;

namespace EmbeddedViews.Controllers
{
    public class UserController : Controller, MyPlugIn.Controllers.IUserController
    {
        private static readonly IList<UserViewModel> _users = new List<UserViewModel>();
        private static readonly IList<RoleViewModel> _roles = new List<RoleViewModel>();

        static UserController()
        {
            _roles.Add(new RoleViewModel { Id = 1, Name = "Manager" });
            _roles.Add(new RoleViewModel { Id = 2, Name = "User" });

            var random = new Random();
            for (int i = 1; i < 11; i++)
            {
                var newUser = new UserViewModel { Id = i, FirstName = "FirstName" + i.ToString(), LastName = "LastName" + i.ToString(), IsActive = true, CreatedDate = DateTime.Now, CreatedUser = "LAFOM5", IsAdmin = Convert.ToBoolean(random.Next(0, 2)), LastUpdateDate = DateTime.Now, LastUpdateUser = "LAFOM5", Login = "SLI\\LASTF" + i.ToString() };

                var numberOfRoles = random.Next(0, 3);

                for (int x = 0; x < numberOfRoles; x++)
                {
                    newUser.Roles.Add(GetNextRole(random, newUser.Roles.Select(r => r.Id)));
                }
                
                _users.Add(newUser);
            }
        }

        private static RoleViewModel GetNextRole(Random random, IEnumerable<int> except)
        {
            int roleId = -1;

            while(roleId == -1 || except.Contains(roleId))
            {
                roleId = random.Next(1, 3);
            }

            return _roles.Single(x => x.Id.Equals(roleId));
        }

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View(_users);
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            UserViewModel userviewmodel = _users.SingleOrDefault(x => x.Id.Equals(id));
            if (userviewmodel == null)
            {
                return HttpNotFound();
            }
            return View(userviewmodel);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View(new UserViewModel());
        }

        //
        // POST: /User/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel userviewmodel)
        {
            if (ModelState.IsValid)
            {
                userviewmodel.CreatedDate = DateTime.Now;
                userviewmodel.LastUpdateDate = DateTime.Now;
                userviewmodel.CreatedUser = "LAFOM5";
                userviewmodel.LastUpdateUser = "LAFOM5";

                foreach (var role in userviewmodel.Roles)
                {
                    role.Name = _roles.Single(x => x.Id.Equals(role.Id)).Name;
                }

                _users.Add(userviewmodel);
                return RedirectToAction("Index");
            }

            return View(userviewmodel);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserViewModel userviewmodel = _users.SingleOrDefault(x => x.Id.Equals(id));
            if (userviewmodel == null)
            {
                return HttpNotFound();
            }
            return View(userviewmodel);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel userviewmodel)
        {
            if (ModelState.IsValid)
            {
                var savedUser = _users.SingleOrDefault(x => x.Id.Equals(userviewmodel.Id));
                userviewmodel.CreatedUser = savedUser.CreatedUser;
                userviewmodel.CreatedDate = savedUser.CreatedDate;
                userviewmodel.LastUpdateDate = DateTime.Now;
                userviewmodel.LastUpdateUser = "LAFOM5";
                _users.Remove(savedUser);

                foreach (var role in userviewmodel.Roles)
                {
                    role.Name = _roles.Single(x => x.Id.Equals(role.Id)).Name;
                }

                _users.Add(userviewmodel);

                return RedirectToAction("Index");
            }
            return View(userviewmodel);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var userviewmodel = _users.SingleOrDefault(x => x.Id.Equals(id));
            
            if (userviewmodel == null)
            {
                return HttpNotFound();
            }

            return View(userviewmodel);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userviewmodel = _users.SingleOrDefault(x => x.Id.Equals(id));
            _users.Remove(userviewmodel);
            return RedirectToAction("Index");
        }

        public ActionResult AddRole()
        {
            return PartialView("RoleRow");
        }
    }
}