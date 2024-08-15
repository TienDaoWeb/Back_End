using TienDaoAPI.DTOs.Bookmarks;
using TienDaoAPI.DTOs.Users;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IBookmarkService
    {
        public Task<bool> CreateBookmarkAsync(CreateBookmarkDTO dto);
        public Task<Bookmark?> GetBookmarkByIdAsync(int id);
        public Task<IEnumerable<Bookmark>?> GetBookmarksByUserIdAsync(int userId);
        public Task<bool> DeleteBookmarkAsync(int id);
        public bool Modifiable(Bookmark bookmark, UserDTO user);
    }
}
