using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.Enums;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;

namespace TienDaoAPI.Repositories
{
    public class CommentRepository : Repository<Comment> , ICommentRepository
    {
        private readonly TienDaoDbContext _dbContext;

        public CommentRepository(TienDaoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
