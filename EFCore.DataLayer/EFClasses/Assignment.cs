using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.DataLayer.EFClasses
{
    [Table("Assignment")]
    public class Assignment
    {
        public Assignment()
        {

        }

        public Assignment(long assignmentID, long projectID, long employeeID, long roleID)
        {
            this.AssignmentID = assignmentID;
            this.ProjectID = projectID;
            this.EmployeeID = employeeID;
            this.RoleID = roleID;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("AssignmentID", Order = 1)]
        public long AssignmentID { get; set; }

        [Column("ProjectID", Order = 2)]
        [Required]
        public long ProjectID { get; set; }
        [ForeignKey("ProjectID")]

        public Project Project { get; set; }

        [Column("EmployeeID", Order = 3)]
        [Required]
        public long EmployeeID { get; set; }
        [ForeignKey("EmployeeID")]

        public Employee Employee { get; set; }

        [Required]
        public long RoleID { get; set; }
        [ForeignKey("RoleID")]

        public Role Role { get; set; }
    }
}
