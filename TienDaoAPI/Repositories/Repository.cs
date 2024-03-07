using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TienDaoAPI.Data;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TienDaoDbContext _dbContext;
        internal DbSet<T> dbSet;

        public Repository(TienDaoDbContext dbContext)
        {
            _dbContext = dbContext;
            dbSet = _dbContext.Set<T>();
        }

        public T? Create(T entity)
        {
            dbSet.Add(entity);
            return entity;
        }

        public T? Get(Expression<Func<T, bool>>? filter = null, string includeProperties = "")
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string includeProperties = "", int size = 10, int page = 1)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (size > 20 || size <= 0)
            {
                size = 20;
                query = query.Skip(size * (page - 1)).Take(size);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }

        public T? GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}
