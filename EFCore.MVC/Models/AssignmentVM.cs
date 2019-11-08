using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFCore.MVC.Models
{
    public class AssignmentVM
    {
        public long Id { get; set; }

        [Required]
        [Range(1,long.MaxValue)]
        public long Project_Id { get; set; }

        public string Project { get; set; }

        [DisplayName("Employee")]
        [Required]
        [Range(1, long.MaxValue)]
        public long Employee_Id { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        public string Department { get; set; }

        [DisplayName("Role")]
        [Required]
        [Range(1, long.MaxValue)]
        public long Role_Id { get; set; }

        public string Role { get; set; }
    }
}
