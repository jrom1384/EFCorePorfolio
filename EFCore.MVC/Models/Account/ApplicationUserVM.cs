using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFCore.MVC.Models
{
    public class ApplicationUserVM
    {
        public string Id { get; set; }

        [DisplayName("Email")]
        [Required]
        public string Email { get; set; }

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

    }
}
