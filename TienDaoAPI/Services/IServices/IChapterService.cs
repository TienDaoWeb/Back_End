using TienDaoAPI.DTOs;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IChapterService
    {
        public Task<Chapter?> CreateChapterAsync(CreateChapterDTO dto);
        public Task<IEnumerable<Chapter?>> GetChaptersByBookIdAsync(int bookId);
        public Task<Chapter?> GetChapterByIdAsync(int chapterId);
        public Task<bool> DeleteChapterAsync(Chapter chapter);
        public Task<bool> DeleteAllChapterAsync(IEnumerable<Chapter> chapters);
        public Task<Chapter?> UpdateChapterAsync(Chapter chapter, UpdateChapterDTO dto);
        public bool Modifiable(Chapter chapter, UserDTO user);
    }
}
