using EFCore.Common;
using EFCore.DTO;
using System.Threading.Tasks;

namespace EFCore.ServiceLayer
{
    public interface IAssignmentService : IGenericService<AssignmentDTO>
    {
        Task<Result<AssignmentPaginatedListDTO>> CreatePageListAsync(AssignmentPageFilterDTO pageFilter);
    }
}
