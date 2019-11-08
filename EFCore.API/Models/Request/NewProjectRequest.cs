using System.ComponentModel.DataAnnotations;

namespace EFCore.API.Models
{
    public class NewProjectRequest
    {
        [Required]
        [StringLength(50)]
        public string Project { get; set; }
    }
}
