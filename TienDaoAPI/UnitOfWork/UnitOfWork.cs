
using TienDaoAPI.Data;
using TienDaoAPI.Repositories;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TienDaoDbContext _dbContext;
        public IUserRepository UserRepository { get; private set; }
        public IRefreshTokenRepository RefreshTokenRepository { get; private set; }

        public UnitOfWork(TienDaoDbContext dbContext)
        {
            _dbContext = dbContext;
            UserRepository = new UserRepository(_dbContext);
            RefreshTokenRepository = new RefreshTokenRepository(_dbContext);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
