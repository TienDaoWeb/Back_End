using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.Repositories
{
    public class ReviewRepository : Repository<Review> , IReviewRepository
    {
        private readonly TienDaoDbContext _dbContext;

        public ReviewRepository(TienDaoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
