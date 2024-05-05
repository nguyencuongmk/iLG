﻿using iLG.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace iLG.Infrastructure.Repositories.Abstractions
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task CreateAsync(TEntity entity);

        Task<IList<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null);

        Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> expression,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
            );

        Task Update(TEntity entity);

        Task Delete(TEntity entity);

        Task<bool> IsExist(Expression<Func<TEntity, bool>> expression);
    }
}