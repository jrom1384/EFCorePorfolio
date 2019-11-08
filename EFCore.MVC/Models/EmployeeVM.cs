using EFCore.Common;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFCore.MVC.Models
{
    public class EmployeeVM
    {
        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("First Name")]
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [DisplayName("Birth Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Status")]
        [Required]
        public bool IsActive { get; set; }

        [DisplayName("Gender")]
        [Required(ErrorMessage = "The Gender field is required")]
        [Range(0,1)]
        public Gender Gender { get; set; }

        [DisplayName("Department")]
        [Required]
        [Range(1,long.MaxValue, ErrorMessage = "The Department field is required")]
        public long Department_Id { get; set; }

        [DisplayName("Department")]
        public string Department { get; set; }
    }
}
