using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFCore.DataLayer.EFClasses
{
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("Age")]
        [Required]
        [Range(1,int.MaxValue)]
        public int Age { get; set; }
    }
}
