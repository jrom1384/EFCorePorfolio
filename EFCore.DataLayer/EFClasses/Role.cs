using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.DataLayer.EFClasses
{
    [Table("Role")]
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("RoleID", Order = 1)]
        public long RoleID { get; set; }

        [Column("RoleName", Order = 2)]
        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
    }
}
