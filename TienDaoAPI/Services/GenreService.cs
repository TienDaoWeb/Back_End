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
        public async Task<Genre?> CreateGenreAsync(GenreRequestDTO genreRequestDTO)
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
            Genre? genre = await _genreRepository.GetByIdAsync(genreId);
            return genre;
        }

        public async Task<Genre?> UpdateGenreAsync(GenreRequestDTO genreRequestDTO, int genreId)
        {
            Genre genre = new Genre
            {
                Name = genreRequestDTO.Name,
                Description = genreRequestDTO.Description
            };
            Genre? result = await _genreRepository.UpdateAsync(genre);
            return result;
        }
        public async Task<IEnumerable<Genre?>> GetAllGenre()
        {
            return await _genreRepository.GetAllAsync();
        }
    }
}
