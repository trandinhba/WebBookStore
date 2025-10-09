using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebBookStore.Repositories
{
    public interface IRepository<T> where T : class
    {
        // Get operations
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        // Add operations
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        // Update operations
        void Update(T entity);

        // Remove operations
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        // Query operations
        IQueryable<T> Query();
        int Count(Expression<Func<T, bool>> predicate = null);
        bool Any(Expression<Func<T, bool>> predicate);
    }
}