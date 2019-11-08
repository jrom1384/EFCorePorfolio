using EFCore.Common;

namespace EFCore.MVC.Models
{
    public class AssignmentPageFilterVM : GenericPage<AssignmentVM>
    {
        public long Department_Id { get; set; }

        public long Role_Id { get; set; }

        public long Project_Id { get; set; }
    }
}
