using EFCore.DataLayer.EFClasses;

namespace EFCore.DataLayer
{
    public class ApiClientRepository : GenericRepository<ApiClient>, IApiClientRepository
    {
        public ApiClientRepository(ApplicationDBContext context) : base(context)
        {

        }
    }
}
