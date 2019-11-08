using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFCore.Razor.Models
{
    public class RoleModel
    {
        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("Role")]
        [Required]
        [MaxLength(50)]
        public string Role { get; set; }
    }
}
