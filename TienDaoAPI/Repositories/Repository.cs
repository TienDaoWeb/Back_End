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

        public async Task<T?> CreateAsync(T entity)
        {
            dbSet.Add(entity);
            await SaveAsync();
            return entity;
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> GetAllbyQueryrAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task DeleteByIdAsync(int id)
        {
            T? entity = await dbSet.FindAsync(id);
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T?> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await SaveAsync();
            return entity;
        }
    }
}
