using System.Linq.Expressions;

namespace Siniauski.WhatIWant.Logic.Interfaces
{
    public interface IRepositoryManager<T> where T : class
    {
        Task CreateAsync(T entity);

        Task CreateRangeAsync(IEnumerable<T> entities);

        IQueryable<T> GetAll();

        Task<T> GetEntityAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetEntityWithoutTrackingAsync(Expression<Func<T, bool>> predicate);

        void Update(T entity);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entity);

        Task SaveChangesAsync();
    }
}