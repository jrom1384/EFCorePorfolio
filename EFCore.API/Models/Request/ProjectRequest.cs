using System.ComponentModel.DataAnnotations;

namespace EFCore.API.Models
{
    public class ProjectRequest
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Project { get; set; }
    }
}
