using System.Linq.Expressions;
using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IChapterService
    {
        public Task<Chapter?> CreateChapterAsync(ChapterPostRequestDTO chapterRequestDTO);
        public Task<IEnumerable<Chapter?>> GetAllChapterAsync(Expression<Func<Chapter, bool>> filter);
        public Task<IEnumerable<Chapter?>> GetAllChapteofStoryrAsync(Expression<Func<Chapter, bool>> filter);
        public Task<Chapter?> GetChapterByIdAsync(int chapterId);
        public Task<Chapter?> GetFinalChapterByIdStoryAsync(int storyId);
        public Task DeleteChapterAsync(Chapter chapter);
        public Task<Chapter?> UpdateChapterAsync(Chapter chapter);

    }
}
