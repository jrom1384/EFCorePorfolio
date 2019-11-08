using EFCore.Common;
using EFCore.DTO;
using System.Threading.Tasks;

namespace EFCore.ServiceLayer
{
    public interface IRoleService : IGenericService<RoleDTO>
    {
        Task<Result<PaginatedListDTO<RoleDTO>>> CreatePageListAsync(PageFilterDTO pageFilter);
    }
}
