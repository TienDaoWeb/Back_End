using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.DTOs.Bookmarks;
using TienDaoAPI.DTOs.Users;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class BookmarkService : IBookmarkService
    {
        private readonly TienDaoDbContext _dbContext;
        private readonly IMapper _mapper;

        public BookmarkService(TienDaoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> CreateBookmarkAsync(CreateBookmarkDTO dto)
        {
            try
            {
                var newBookmark = _mapper.Map<Bookmark>(dto);
                _dbContext.Bookmarks.Add(newBookmark);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteBookmarkAsync(int id)
        {
            try
            {
                var bookmark = await _dbContext.Bookmarks.FirstOrDefaultAsync(x => x.Id == id);

                if (bookmark is null)
                {
                    return false;
                }
                _dbContext.Bookmarks.Remove(bookmark);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Bookmark?> GetBookmarkByIdAsync(int id)
        {
            return await _dbContext.Bookmarks.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Bookmark>?> GetBookmarksByUserIdAsync(int userId)
        {
            return await _dbContext.Bookmarks
                .Include(b => b.Book).ThenInclude(b => b!.Author)
                .Where(c => c.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public bool Modifiable(Bookmark bookmark, UserDTO user)
        {
            return bookmark.UserId == user.Id;
        }
    }
}
