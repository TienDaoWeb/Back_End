using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.Repositories
{
    public class EmojiRepository : Repository<Emoji> , IEmojiRepository
    {
        private readonly TienDaoDbContext _dbContext;

        public EmojiRepository(TienDaoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
