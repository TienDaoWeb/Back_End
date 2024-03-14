using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IGenreService
    {
        public Task<Genre?> CreateGenreAsync(GenreRequest genreRequestDTO);
        public Task<Genre?> GetGenreByIdAsync(int genreId);
        public Task<IEnumerable<Genre>> GetAllGenresAsync();
        public Task DeleteGenreAsync(int genreId);
        public Task<Genre?> UpdateGenreAsync(Genre genre);
    }
}
