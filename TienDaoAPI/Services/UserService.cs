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


        public Task<User> CreateNewUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool CheckPassword(User user, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public Task<User?> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
