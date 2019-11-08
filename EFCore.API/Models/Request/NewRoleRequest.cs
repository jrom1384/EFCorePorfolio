using System.ComponentModel.DataAnnotations;

namespace EFCore.API.Models
{
    public class NewRoleRequest
    {
        [Required]
        [StringLength(50)]
        public string Role { get; set; }
    }
}
