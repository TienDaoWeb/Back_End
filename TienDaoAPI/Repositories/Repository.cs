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

        public async Task<T> CreateAsync(T entity)
        {
            dbSet.Add(entity);
            await SaveAsync();
            return entity;
        }

        public async Task<T?> Find(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
