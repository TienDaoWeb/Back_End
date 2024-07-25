using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        private readonly TienDaoDbContext _dbContext;
        public AuthorRepository(TienDaoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
