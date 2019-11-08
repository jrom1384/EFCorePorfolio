using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFCore.MVC.Models
{
    public class ProjectVM
    {
        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("Project")]
        [Required]
        [MaxLength(100)]
        public string Project { get; set; }

        public int MemberCount { get; set; }
    }
}
