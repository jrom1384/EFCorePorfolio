namespace EFCore.DTO
{
    public class AssignmentDTO
    {
        public long Id { get; set; }

        public long Project_Id { get; set; }

        public string Project { get; set; }

        public long Employee_Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Department { get; set; }

        public long Role_Id { get; set; }

        public string Role { get; set; }
    }
}
