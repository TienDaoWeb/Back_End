using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IGenreService
    {
        public Task<Genre?> CreateGenreAsync(GenreRequestDTO genreRequestDTO);
        public Task<Genre?> GetGenreByIdAsync(int genreId);
        public Task DeleteGenreAsync(int genreId);
        public Task<Genre?> UpdateGenreAsync(GenreRequestDTO genreRequestDTO, int genreId);
    }
}
