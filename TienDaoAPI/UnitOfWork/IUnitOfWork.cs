using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IGenreRepository GenreRepository { get; }
        IStoryRepository StoryRepository { get; }
        Task SaveAsync();
    }
}
