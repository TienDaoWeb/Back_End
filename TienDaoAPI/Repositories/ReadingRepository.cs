using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.Repositories
{
    public class ReadingRepository : Repository<Reading>, IReadingRepository
    {
        private readonly TienDaoDbContext _dbContext;

        public ReadingRepository(TienDaoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
