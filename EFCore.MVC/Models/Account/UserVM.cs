using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.MVC.Models
{
    public class UserVM
    {
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("Age")]
        [Required]
        [Range(1, int.MaxValue)]
        public int Age { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
