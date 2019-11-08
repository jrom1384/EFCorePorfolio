using System.ComponentModel.DataAnnotations;

namespace EFCore.API.Models
{
    public class ApiClientRegistrationRequest : ApiClientLoginRequest
    {

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }
    }
}
