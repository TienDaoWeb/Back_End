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

        public async Task<bool> CreateGenreAsync(Genre genre)
        {
            Genre? result = await _genreRepository.CreateAsync(genre);
            return result != null;
        }

        public async Task<bool> DeleteGenreAsync(int genreId)
        {
            try
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);
                if (genre != null)
                {
                    await _genreRepository.DeleteAsync(genre);
                    return true;
                }
                return false;
            }
            catch
            {
                return true;
            }
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _genreRepository.GetAllAsync();
        }
    }
}
