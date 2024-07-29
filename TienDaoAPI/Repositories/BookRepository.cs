using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly TienDaoDbContext _dbContext;

        public BookRepository(TienDaoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
