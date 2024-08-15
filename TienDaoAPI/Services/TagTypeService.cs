using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class TagTypeService : ITagTypeService
    {
        private readonly TienDaoDbContext _dbContext;
        public TagTypeService(TienDaoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateTagTypeAsync(TagType tagType)
        {
            try
            {
                var result = _dbContext.TagTypes.Add(tagType);
                await _dbContext.SaveChangesAsync();
                return result is not null;
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
                var tag = await _dbContext.TagTypes.FirstOrDefaultAsync(x => x.Id == id);

                if (tag is null)
                {
                    return false;
                }
                _dbContext.TagTypes.Remove(tag);
                await _dbContext.SaveChangesAsync();
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
            return await _dbContext.TagTypes.ToListAsync();
        }
    }
}
