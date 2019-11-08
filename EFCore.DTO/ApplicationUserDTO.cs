using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DTO
{
    public class ApplicationUserDTO : IdentityResultDTO
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }
    }
}
