using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class GenreService : IGenreService
    {
        private readonly TienDaoDbContext _dbContext;
        public GenreService(TienDaoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateGenreAsync(Genre genre)
        {
            try
            {
                var result = _dbContext.Genres.Add(genre);
                await _dbContext.SaveChangesAsync();
                return result is not null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return true;
            }
        }

        public async Task<bool> DeleteGenreAsync(int id)
        {
            try
            {
                var genre = await _dbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);

                if (genre is null)
                {
                    return false;
                }
                _dbContext.Genres.Remove(genre);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return true;
            }
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _dbContext.Genres.ToListAsync();
        }
    }
}
