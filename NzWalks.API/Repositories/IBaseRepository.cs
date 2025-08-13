using System.Linq.Expressions;

namespace NzWalks.API.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, 
            Expression<Func<T, object>>? orderBy = null, 
            bool? isAscending = true, 
            int pageNumber = 1,
            int pageSize = 1000, 
            params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync(Guid id, T entity);
        Task<T?> DeleteAsync(Guid id);
    }
}
