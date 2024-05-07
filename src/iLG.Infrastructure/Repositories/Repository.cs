using iLG.Domain.Abstractions;
using iLG.Infrastructure.Data;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace iLG.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly ILGDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ILGDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> expression,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
            )
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes(query);
            }

            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression)
        {
            IQueryable<TEntity> query = _dbSet;
            return await query.AnyAsync(expression);
        }
    }
}