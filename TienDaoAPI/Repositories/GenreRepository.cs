using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.Repositories
{
    public class GenreRepository: Repository<Genre> , IGenreRepository
    {
        private readonly TienDaoDbContext _dbContext;

        public GenreRepository(TienDaoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
