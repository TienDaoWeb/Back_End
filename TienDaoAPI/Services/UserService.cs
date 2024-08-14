using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.Enums;
using TienDaoAPI.Helpers;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Services
{
    public class UserService : IUserService
    {
        private readonly TienDaoDbContext _dbContext;

        public UserService(TienDaoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<User?>> FilterUser(UserFilter filter)
        {
            var filterExpression = ExpressionProvider<User>.BuildUserFilter(filter);

            var user = _dbContext.Users.Where(filterExpression);

            var sortExpression = ExpressionProvider<User>.GetSortExpression(filter.SortBy);
            if (filter.SortBy!.StartsWith("-"))
            {
                user = user.OrderByDescending(sortExpression);
            }
            else
            {
                user = user.OrderBy(sortExpression);
            }
            return await user.ToListAsync();
        }


        public async Task<int> LockUser(User user)
        {
            try
            {
                if (user.Status == AccountStatusEnum.BLOCKED)
                {
                    return 1;
                }
                user.Status = AccountStatusEnum.BLOCKED;
                await _dbContext.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 3;
            }
        }

        public async Task<bool> UnlockUser(User user)
        {
            try
            {
                if (user.EmailConfirmed)
                {
                    user.Status = AccountStatusEnum.ACTIVED;
                }
                else
                {
                    user.Status = AccountStatusEnum.UNVERIFIED;
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
