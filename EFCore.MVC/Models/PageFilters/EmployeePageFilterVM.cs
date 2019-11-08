using EFCore.Common;

namespace EFCore.MVC.Models
{
    public class EmployeePageFilterVM : GenericPage<EmployeeVM>
    {
        public Gender? Gender { get; set; }

        public bool? IsActive { get; set; }

        public long Department_Id { get; set; }
    }
}
