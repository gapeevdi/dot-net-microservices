using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ordering.Domain.Common;

namespace Ordering.Application.Contracts.Persistance
{
    public interface IAsyncRepository<T>
        where T : EntityBase
    {
        Task<IReadOnlyList<T>> GetAll();
        Task<IReadOnlyList<T>> Get(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> Get(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string include = null, bool trackEntity = true);

        Task<IReadOnlyList<T>> Get(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            IReadOnlyList<Expression<Func<T, object>>> includes = null, 
            bool trackEntity = true);

        Task<T> GetById(int id);

        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
