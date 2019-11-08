using System.ComponentModel.DataAnnotations;

namespace EFCore.API.Models
{
    public class RoleRequest
    {
        [Required]
        [Range(1,long.MaxValue)]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }
    }
}
