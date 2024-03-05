using System.Linq.Expressions;

namespace TienDaoAPI.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetById(int id);
        Task<T?> Find(Expression<Func<T, bool>> predicate);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entiry);
        Task RemoveAsync(T entity);
        Task SaveAsync();
    }
}
