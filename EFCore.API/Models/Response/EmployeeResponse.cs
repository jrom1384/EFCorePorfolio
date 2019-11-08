namespace EFCore.API.Models
{
    public class EmployeeResponse
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateOfBirth { get; set; }

        public string Gender { get; set; }

        public bool IsActive { get; set; }

        public long Department_Id { get; set; }

        public string Department { get; set; }
    }
}
