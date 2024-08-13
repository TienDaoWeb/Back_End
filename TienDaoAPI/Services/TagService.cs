using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            try
            {
                var result = await _tagRepository.CreateAsync(tag);
                return result != null;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            try
            {
                var tag = await _tagRepository.GetAsync(t => t.Id == id);
                if (tag == null)
                {
                    return false;
                }
                await _tagRepository.DeleteAsync(tag);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            return await _tagRepository.GetAllAsync();
        }
    }
}
