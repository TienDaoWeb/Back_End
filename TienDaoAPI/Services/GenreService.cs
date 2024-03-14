using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }
        public async Task<Genre?> CreateGenreAsync(GenreRequest genreRequestDTO)
        {
            var newGenre = new Genre
            {
                Name = genreRequestDTO.Name,
                Description = genreRequestDTO.Description
            };

            Genre? result = await _genreRepository.CreateAsync(newGenre);
            return result;
        }

        public async Task DeleteGenreAsync(int genreId)
        {
            await _genreRepository.DeleteByIdAsync(genreId);
        }

        public async Task<Genre?> GetGenreByIdAsync(int genreId)
        {
            return await _genreRepository.GetAsync(g => g.Id == genreId);
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _genreRepository.GetAllAsync();
        }

        public async Task<Genre?> UpdateGenreAsync(Genre genre)
        {
            return await _genreRepository.UpdateAsync(genre);
        }
    }
}
