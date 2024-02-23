using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly TienDaoDbContext _dbContext;

        public UserRepository(TienDaoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
