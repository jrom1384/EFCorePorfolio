using System.ComponentModel.DataAnnotations;

namespace EFCore.API.Models
{
    public class NewDepartmentRequest
    {
        [Required]
        [StringLength(50)]
        public string Department { get; set; }
    }
}
