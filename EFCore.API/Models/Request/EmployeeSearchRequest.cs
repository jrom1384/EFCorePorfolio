namespace EFCore.API.Models
{
    public class EmployeeSearchRequest : SearchRequest
    {
        public string Gender { get; set; }

        public string IsActive { get; set; }

        public long Department_Id { get; set; }
    }
}
