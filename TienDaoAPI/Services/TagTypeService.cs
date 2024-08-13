using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class TagTypeService : ITagTypeService
    {
        private readonly ITagTypeRepository _tagTypeRepository;

        public TagTypeService(ITagTypeRepository tagTypeRepository)
        {
            _tagTypeRepository = tagTypeRepository;
        }

        public async Task<bool> CreateTagTypeAsync(TagType tagType)
        {
            try
            {
                var result = await _tagTypeRepository.CreateAsync(tagType);
                return result != null;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTagTypeAsync(int id)
        {
            try
            {
                var tag = await _tagTypeRepository.GetAsync(t => t.Id == id);
                if (tag == null)
                {
                    return false;
                }
                await _tagTypeRepository.DeleteAsync(tag);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<IEnumerable<TagType>> GetTagTypesAsync()
        {
            return await _tagTypeRepository.GetAllAsync();
        }
    }
}
