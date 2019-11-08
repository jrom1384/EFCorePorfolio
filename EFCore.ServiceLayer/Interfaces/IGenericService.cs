using EFCore.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCore.ServiceLayer
{
    public interface IGenericService<TDTO> where TDTO : class
    {
        Task<Result<TDTO>> AddAsync(TDTO dto);

        Task<Result<IEnumerable<TDTO>>> AddRangeAsync(IEnumerable<TDTO> dtos);

        Task<Result<IEnumerable<TDTO>>> DeleteRangeAsync(IEnumerable<TDTO> dtos);

        Task<Result<IEnumerable<TDTO>>> UpdateRangeAsync(IEnumerable<TDTO> dtos);

        Task<Result<TDTO>> UpdateAsync(TDTO dto);

        Task<Result<TDTO>> DeleteAsync(TDTO dto);

        Task<Result<TDTO>> DeleteByIdAsync(long id);

        Task<Result<TDTO>> FindByIdAsync(long id);

        Task<Result<List<TDTO>>> GetListAsync();

        Task<Result<TDTO>> FirstOrDefaultAsync(Expression<Func<TDTO, bool>> predicate);

        Task<Result<TDTO>> SingleOrDefaultAsync(Expression<Func<TDTO, bool>> predicate);

        Task<Result<bool>> AnyAsync(Expression<Func<TDTO, bool>> predicate);
    }
}
