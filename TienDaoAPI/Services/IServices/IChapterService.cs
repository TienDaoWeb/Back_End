using System.Linq.Expressions;
using TienDaoAPI.DTOs;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IChapterService
    {
        public Task<Chapter?> CreateChapterAsync(CreateChapterDTO dto);
        public Task<IEnumerable<Chapter?>> GetAllChapterAsync(Expression<Func<Chapter, bool>> filter);
        public Task<IEnumerable<Chapter?>> GetAllChapterOfBookAsync(Expression<Func<Chapter, bool>> filter);
        public Task<Chapter?> GetChapterByIdAsync(int chapterId);
        public Task<Chapter?> GetFinalChapterByIdBookAsync(int storyId);
        public Task DeleteChapterAsync(Chapter chapter);
        public Task DeleteAllChapterAsync(IEnumerable<Chapter> chapters);
        public Task<Chapter?> UpdateChapterAsync(Chapter chapter);

    }
}
