using System.ComponentModel.DataAnnotations;

namespace EFCore.API.Models
{
    public class EmployeeRequest
    {
        [Required]
        [Range(1,long.MaxValue)]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long Department_Id { get; set; }
    }
}
