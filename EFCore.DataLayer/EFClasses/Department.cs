using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.DataLayer.EFClasses
{
    [Table("Department")]
    public class Department
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("DepartmentID", Order = 1)]
        public long DepartmentID { get; set; }

        [Column("DepartmentName", Order = 2)]
        [Required]
        [MaxLength(50)]
        public string DepartmentName { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
