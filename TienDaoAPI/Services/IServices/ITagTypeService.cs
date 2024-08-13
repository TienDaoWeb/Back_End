using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface ITagTypeService
    {
        public Task<bool> CreateTagTypeAsync(TagType tagType);
        public Task<IEnumerable<TagType>> GetTagTypesAsync();
        public Task<bool> DeleteTagTypeAsync(int id);
    }
}
