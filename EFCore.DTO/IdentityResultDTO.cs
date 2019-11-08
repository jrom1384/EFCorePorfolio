using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DTO
{
    public class IdentityResultDTO
    {
        public IdentityResultDTO()
        {

        }

        public IdentityResultDTO(bool succeeded)
        {
            this.Succeeded = succeeded;
        }


        public bool Succeeded { get; set; }

        public IList<string> Errors { get; set; }

    }
}
