using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.MVC.Models
{
    public class RegisterUserVM
    {
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("Age")]
        [Required]
        public int Age { get; set; }

        [DisplayName("Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Password")]
        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public bool RequireConfirmedEmail { get; set; }
    }
}
