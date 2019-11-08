using EFCore.Common;
using EFCore.DTO;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCore.ServiceLayer
{
    public interface IEmployeeService : IGenericService<EmployeeDTO>
    {
        Task<Result<EmployeeDTO>> FirstOrDefaultIncludeDepartmentAsync(Expression<Func<EmployeeDTO, bool>> predicate);

        Task<Result<EmployeePaginatedListDTO>> CreatePageListAsync(EmployeePageFilterDTO pageFilter);

        Task<Result<EmployeeDTO>> PatchEmployeeAsync(long id, JsonPatchDocument<EmployeeDTO> patchDoc);

        Task<Result<List<EmployeeDTO>>> GetListAsync(long id);
    }
}
