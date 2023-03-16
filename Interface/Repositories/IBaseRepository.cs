using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dansnom.Interface.Repositories
{
    public interface IBaseRepository<T>
    {
        Task<T> CreateAsync(T entity);
        Task SaveChangesAsync();
        Task<bool> DeleteAsync(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task<T> GetAsync(int id);
        Task<T> UpdateAsync(T entity);
    }
}