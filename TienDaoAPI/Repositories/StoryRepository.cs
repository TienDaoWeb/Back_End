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
    }
}
