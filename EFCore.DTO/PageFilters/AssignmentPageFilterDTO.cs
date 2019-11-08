namespace EFCore.DTO
{
    public class AssignmentPageFilterDTO : PageFilterDTO
    {
        public long Department_Id { get; set; }

        public long Role_Id { get; set; }

        public long Project_Id { get; set; }
    }
}
