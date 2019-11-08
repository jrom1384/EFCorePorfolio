using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCore.DataLayer
{
    public interface IGenericRepository<TEntity> 
        where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity item);

        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> items);

        Task<TEntity> DeleteAsync(TEntity item);

        Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> items);

        Task<TEntity> DeleteByIdAsync(long id);

        Task<TEntity> UpdateAsync(TEntity item);

        Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> items);

        Task<int> CommitAsync();

        Task<TEntity> FindByIdAsync(long id);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Select(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        DbSet<TEntity> GetTable();
    }
}
