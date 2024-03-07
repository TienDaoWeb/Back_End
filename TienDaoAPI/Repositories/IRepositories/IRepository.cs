using System.Linq.Expressions;

namespace TienDaoAPI.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string includeProperties = "", int size = 20, int page = 1);
        T? GetById(int id);
        T? Get(Expression<Func<T, bool>> filter, string includeProperties = "");
        T? Create(T entity);
        void Remove(T entity);
    }
}
