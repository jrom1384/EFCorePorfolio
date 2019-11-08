using EFCore.DataLayer.EFClasses;

namespace EFCore.DataLayer
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDBContext context) : base(context)
        {

        }
    }
}
