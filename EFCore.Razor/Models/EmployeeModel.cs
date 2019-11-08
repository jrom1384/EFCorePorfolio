using EFCore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFCore.Razor.Models
{
    public class EmployeeModel
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

        [DisplayName("Gender")]
        [Required]
        public Gender Gender { get; set; }

        [DisplayName("Status")]
        [Required]
        public bool IsActive { get; set; }

        [DisplayName("Department")]
        [Required]
        public long Department_Id { get; set; }

        [DisplayName("Department")]
        public string Department { get; set; }

        public List<DepartmentModel> Departments { get; set; }
    }
}
