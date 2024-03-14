using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.Repositories
{
    public class StoryRepository : Repository<Story>, IStoryRepository
    {
        private readonly TienDaoDbContext _dbContext;

        public StoryRepository(TienDaoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<IEnumerable<Story>> GetAllAsync()
        {
            return await dbSet.Include(s => s.Genre).ToListAsync();
        }

        public override async Task<Story?> GetByIdAsync(int id)
        {
            return await dbSet.Include(s => s.Genre).FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
