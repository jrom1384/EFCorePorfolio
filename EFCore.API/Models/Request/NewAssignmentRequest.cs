using System.ComponentModel.DataAnnotations;

namespace EFCore.API.Models
{
    public class NewAssignmentRequest
    {
        [Required]
        [Range(1,long.MaxValue)]
        public long Project_Id { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long Employee_Id { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long Role_Id { get; set; }
    }
}
