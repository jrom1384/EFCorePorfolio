using EFCore.Common;
using EFCore.DTO;
using System.Threading.Tasks;

namespace EFCore.ServiceLayer
{
    public interface IDepartmentService : IGenericService<DepartmentDTO>
    {
        Task<Result<PaginatedListDTO<DepartmentDTO>>> CreatePageListAsync(PageFilterDTO pageFilter);
    }
}
