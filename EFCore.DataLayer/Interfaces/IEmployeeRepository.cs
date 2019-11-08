using EFCore.DataLayer.EFClasses;

namespace EFCore.DataLayer
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        /* Add employee specific signatures for DI use. */
    }
}
