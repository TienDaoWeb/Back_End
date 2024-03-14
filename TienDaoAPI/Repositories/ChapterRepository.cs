using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.Repositories
{
    public class ChapterRepository : Repository<Chapter>, IChapterRepository
    {
        private readonly TienDaoDbContext _dbContext;
        public ChapterRepository(TienDaoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
