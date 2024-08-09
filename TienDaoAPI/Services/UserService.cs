using Microsoft.AspNetCore.Identity;
using TienDaoAPI.Enums;
using TienDaoAPI.Helpers;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }


        public async Task<User?> GetUserByIdAsync(int id)
        {
            try
            {
                return await _userRepository.GetAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<User?>> GetAllUsers(UserFilter filter)
        {
            var filterExpression = ExpressionProvider<User>.BuildUserFilter(filter);
            var sortExpression = filter.SortBy == null ? null : ExpressionProvider<User>.GetSortExpression(filter.SortBy);
            return await _userRepository.FilterAsync(filterExpression, null, sortExpression);
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
                await _userRepository.SaveAsync();
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
                await _userRepository.SaveAsync();
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
