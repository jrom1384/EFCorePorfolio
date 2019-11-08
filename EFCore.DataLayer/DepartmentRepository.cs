using EFCore.DataLayer.EFClasses;

namespace EFCore.DataLayer
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDBContext context) : base(context)
        {
         
        }
    }
}
