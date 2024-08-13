using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface ITagService
    {
        public Task<bool> CreateTagAsync(Tag tag);
        public Task<IEnumerable<Tag>> GetTagsAsync();
        public Task<bool> DeleteTagAsync(int id);
    }
}
