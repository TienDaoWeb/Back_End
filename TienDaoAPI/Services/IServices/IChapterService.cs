using TienDaoAPI.DTOs.Chapters;
using TienDaoAPI.DTOs.Users;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IChapterService
    {
        public Task<bool> CreateChapterAsync(CreateChapterDTO dto);
        public Task<IEnumerable<Chapter?>> GetChaptersByBookIdAsync(int bookId);
        public Task<Chapter?> GetChapterByIdAsync(int chapterId);
        public Task<bool> DeleteChapterAsync(Chapter chapter);
        public Task<bool> UpdateChapterAsync(Chapter chapter, UpdateChapterDTO dto);
        public bool Modifiable(Chapter chapter, UserDTO user);
    }
}
