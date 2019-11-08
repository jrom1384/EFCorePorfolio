using EFCore.DataLayer.EFClasses;

namespace EFCore.DataLayer
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        /* Add project specific functions for DI use. */
    }
}
