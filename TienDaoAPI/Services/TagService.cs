using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class TagService : ITagService
    {
        private readonly TienDaoDbContext _dbContext;

        public TagService(TienDaoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            try
            {
                var result = _dbContext.Tags.Add(tag);
                await _dbContext.SaveChangesAsync();
                return result is not null;
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
                var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
                if (tag is null)
                {
                    return false;
                }

                _dbContext.Remove(tag);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            return await _dbContext.Tags.ToListAsync();
        }
    }
}
