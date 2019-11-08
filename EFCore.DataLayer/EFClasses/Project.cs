using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.DataLayer.EFClasses
{
    [Table("Project")]
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("ProjectID", Order = 1)]
        public long ProjectID { get; set; }

        [Column("ProjectName", Order = 2)]
        [Required]
        [MaxLength(100)]
        public string ProjectName { get; set; }

        [DefaultValue(0)]
        public int MemberCount { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
    }
}
