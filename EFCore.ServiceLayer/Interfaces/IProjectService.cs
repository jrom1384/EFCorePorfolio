using EFCore.Common;
using EFCore.DTO;
using System.Threading.Tasks;

namespace EFCore.ServiceLayer
{
    public interface IProjectService : IGenericService<ProjectDTO>
    {
        Task<Result<PaginatedListDTO<ProjectDTO>>> CreatePageListAsync(PageFilterDTO pageFilter);

        Task<Result<AssignmentPaginatedListDTO>> CreatePageListAsync(AssignmentPageFilterDTO pageFilter);
    }
}
