using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPlugIn.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Roles = new List<RoleViewModel>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public string LastUpdateUser { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        [CustomValidation(typeof(UserViewModel), "ValidateRoles")]
        public IList<RoleViewModel> Roles { get; set; }
        public string RolesDisplay
        {
            get { return string.Join(", ", Roles.Select(x => x.Name)); }
        }

        public static ValidationResult ValidateRoles(IList<RoleViewModel> roles)
        {
            if (roles.Select(x => x.Id).Distinct().Count() != roles.Count)
                return new ValidationResult("Cannot select the same role more than once");

            return ValidationResult.Success;
        }
    }
}