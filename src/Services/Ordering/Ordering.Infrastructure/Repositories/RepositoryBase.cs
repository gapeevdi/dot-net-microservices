using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistance;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure
{
    public class RepositoryBase<T> : IAsyncRepository<T>
        where T : EntityBase
    {
        protected OrderContext Context { get; }

        public RepositoryBase(OrderContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> Get(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> Get(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string include = null, bool trackEntity = true)
        {
            IQueryable<T> query = Context.Set<T>();
            if (trackEntity == false)
            {
                query = query.AsNoTracking();
            }

            if (string.IsNullOrEmpty(include) == false)
            {
                query = query.Include(include);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> Get(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, IReadOnlyList<Expression<Func<T, object>>> includes = null, bool trackEntity = true)
        {
            IQueryable<T> query = Context.Set<T>();
            if (trackEntity == false)
            {
                query = query.AsNoTracking();
            }

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public async Task<T> Add(T entity)
        {
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}
