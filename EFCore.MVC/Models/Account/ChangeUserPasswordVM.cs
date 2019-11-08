using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.MVC.Models
{
    public class ChangeUserPasswordVM
    {
        [Required]
        public string Id { get; set; }

        [DisplayName("Current Password")]
        [Required]
        public string CurrentPassword { get; set; }

        [DisplayName("Password")]
        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
