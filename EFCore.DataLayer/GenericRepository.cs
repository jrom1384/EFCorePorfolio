using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCore.DataLayer
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected ApplicationDBContext _context = null;
        protected DbSet<TEntity> _table = null;

        public GenericRepository(ApplicationDBContext context)
        {
            _context = context;
            _table = _context.Set<TEntity>();
        }

        public DbSet<TEntity> GetTable()
        {
            return _table;
        }

        public async Task<TEntity> AddAsync(TEntity item)
        {
            await _table.AddAsync(item);
            await CommitAsync();
            return item;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<TEntity> DeleteAsync(TEntity item)
        {
            _table.Remove(item);
            await CommitAsync();
            return item;
        }

        public async Task<TEntity> DeleteByIdAsync(long id)
        {
            TEntity item = await FindByIdAsync(id);
            if (item != null)
            {
                _table.Remove(item);
                await CommitAsync();
            }
            
            return item;
        }

        public async Task<TEntity> FindByIdAsync(long id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _table.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _table;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity item)
        {
            _table.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await CommitAsync();
            return item;
        }

        public async Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _table.Where(predicate).ToListAsync();
        }

        public IQueryable<TEntity> Select(Expression<Func<TEntity, bool>> predicate)
        {
            return _table.Where(predicate).Select(i => i);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _table.AnyAsync(predicate);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _table.SingleOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> items)
        {
            await _table.AddRangeAsync(items);
            await CommitAsync();
            return items;
        }

        public async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> items)
        {
            _table.UpdateRange(items);
            await CommitAsync();
            return items;
        }

        public async Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> items)
        {
            _table.RemoveRange(items);
            await CommitAsync();
            return items;
        }
    }
}
