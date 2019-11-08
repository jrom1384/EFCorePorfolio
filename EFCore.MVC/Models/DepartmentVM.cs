using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFCore.MVC.Models
{
    public class DepartmentVM
    {
        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("Department")]
        [Required]
        [MaxLength(50)]
        public string Department { get; set; }
    }
}
