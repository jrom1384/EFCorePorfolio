using EFCore.Common;

namespace EFCore.DTO
{
    public class EmployeePageFilterDTO : PageFilterDTO
    {
        public Gender? Gender { get; set; }

        public bool? IsActive { get; set; }

        public long Department_Id { get; set; }
    }
}
