using System.ComponentModel.DataAnnotations;

namespace EFCore.API.Models
{
    public class ApiClientRequest : ApiClientLoginRequest
    {
        [Required]
        [Range(1,long.MaxValue)]
        public long Id { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }
    }
}
