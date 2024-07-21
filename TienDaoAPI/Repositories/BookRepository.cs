using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly TienDaoDbContext _dbContext;

        public BookRepository(TienDaoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await dbSet.Include(s => s.Genre).ToListAsync();
        }

        public override async Task<Book?> GetByIdAsync(int id)
        {
            return await dbSet.Include(s => s.Genre).FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
