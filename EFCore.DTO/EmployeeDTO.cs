using EFCore.Common;
using System;
using System.Collections.Generic;

namespace EFCore.DTO
{
    public class EmployeeDTO
    {
        public long Id { get; set; }
      
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public bool IsActive { get; set; }

        public long Department_Id { get; set; }
        
        public string Department { get; set; }

    }
}
