﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TienDaoAPI.Data;
using TienDaoAPI.Extensions;
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

        public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query.AsNoTracking();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                var includePropertiesArray = includeProperties.Split(
                    new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries);
                foreach (var includeProperty in includePropertiesArray)
                {
                    var trimmedIncludeProperty = includeProperty.Trim().ToPascalCase();
                    query = query.Include(trimmedIncludeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                var includePropertiesArray = includeProperties.Split(
                    new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries);
                foreach (var includeProperty in includePropertiesArray)
                {
                    var trimmedIncludeProperty = includeProperty.Trim().ToPascalCase();
                    query = query.Include(trimmedIncludeProperty);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task DeleteByIdAsync(int id)
        {
            T? entity = await dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T?> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            dbSet.Update(entity);
            await SaveAsync();
            return entity;
        }
    }
}
