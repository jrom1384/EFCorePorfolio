using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DTO
{
    public class ForgotPasswordDTO : IdentityResultDTO
    {
        public ForgotPasswordDTO()
        {

        }

        public ForgotPasswordDTO(bool succeeded, string code = null)
            : base(succeeded)
        {
            this.Code = code;
        }

        public ForgotPasswordDTO(bool succeeded, string Id, string email, string firstName, string code = null)
           : base(succeeded)
        {
            this.Id = Id;
            this.Email = email;
            this.FirstName = firstName;
            this.Code = code;
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string Code { get; set; }
    }
}
