using System.ComponentModel.DataAnnotations;

namespace EFCore.API.Models
{
    public class ApiClient
    {
        public long Id { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string Username { get; set; }
    }
}
