using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IGenreService
    {
        public Task<bool> CreateGenreAsync(Genre genre);
        public Task<IEnumerable<Genre>> GetAllGenresAsync();
        public Task<bool> DeleteGenreAsync(int id);
    }
}
