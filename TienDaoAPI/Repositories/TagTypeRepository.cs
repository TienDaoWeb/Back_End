using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.Repositories
{
    public class TagTypeRepository : Repository<TagType>, ITagTypeRepository
    {
        private readonly TienDaoDbContext _dbContext;

        public TagTypeRepository(TienDaoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
