using Microsoft.AspNetCore.Identity;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

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

        public async Task<IEnumerable<User?>> GetAllUsers()
        {
            return await _userRepository.GetAllAsync();
        }
    }
}
