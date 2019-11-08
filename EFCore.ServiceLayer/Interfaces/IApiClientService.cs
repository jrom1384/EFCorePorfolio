using EFCore.Common;
using EFCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCore.ServiceLayer
{
    public interface IApiClientService
    {
        Task<Result<ApiClientDTO>> AddAsync(ApiClientDTO dto, string password);

        Task<Result<ApiClientDTO>> UpdateAsync(ApiClientDTO dto, string password = null);

        Task<Result<ApiClientDTO>> AuthenticateAsync(string username, string password);

        Task<Result<ApiClientDTO>> DeleteAsync(ApiClientDTO dto);

        Task<Result<ApiClientDTO>> DeleteByIdAsync(long id);

        Task<Result<ApiClientDTO>> FindByIdAsync(long id);

        Task<Result<List<ApiClientDTO>>> GetListAsync();

        Task<Result<ApiClientDTO>> FirstOrDefaultAsync(Expression<Func<ApiClientDTO, bool>> predicate);

        Task<Result<ApiClientDTO>> SingleOrDefaultAsync(Expression<Func<ApiClientDTO, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<ApiClientDTO, bool>> predicate);
    }
}
