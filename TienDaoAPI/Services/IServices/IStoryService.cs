using System.Linq.Expressions;
using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IStoryService
    {
        public Task<Story?> CreateStoryAsync(StoryRequestDTO storyRequestDTO);
        public Task<IEnumerable<Story?>> GetAllStoriesAsync(Expression<Func<Story, bool>> filter);
        public Task<Story?> GetStoryByIdAsync(int storyId);
        public Task DeleteStoryAsync(Story story);
        public Task<Story?> UpdateStoryAsync(Story story);
    }
}
