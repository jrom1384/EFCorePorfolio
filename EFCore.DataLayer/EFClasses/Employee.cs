using EFCore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.DataLayer.EFClasses
{
    [Table("Employee")]
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("EmployeeID", Order = 1)]
        public long EmployeeID { get; set; }

        [Column("FirstName", Order = 2)]
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Column("LastName", Order = 3)]
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Column("DateOfBirth", TypeName = "Date", Order = 4)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Column("Gender", Order = 5)]
        [Required]
        public Gender Gender { get; set; }

        [Column("IsActive", Order = 6)]
        [Required]
        public bool IsActive { get; set; }

        [Required]
        public long DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]

        public Department Department { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
    }
}
