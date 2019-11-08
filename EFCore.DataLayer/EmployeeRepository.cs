using EFCore.DataLayer.EFClasses;

namespace EFCore.DataLayer
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDBContext context) : base(context)
        {

        }
    }
}
