using System.ComponentModel.DataAnnotations;

namespace EFCore.API.Models
{
    public class ApiClientLoginRequest
    {
        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }
    }
}
