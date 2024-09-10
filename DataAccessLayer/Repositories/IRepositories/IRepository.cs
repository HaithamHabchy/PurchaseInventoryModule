using System.Linq.Expressions;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter, string includeProperties = null);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, string includeProperties = null);
        Task CreateAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
