using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DTO
{
    public class RegisterUserDTO : IdentityResultDTO
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool RequireConfirmedEmail { get; set; }
    }
}
