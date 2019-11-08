using EFCore.DataLayer.EFClasses;

namespace EFCore.DataLayer
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        /* Add department specific functions for DI use. */
    }
}
