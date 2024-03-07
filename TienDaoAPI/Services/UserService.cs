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

        //public async Task<User> CreateNewUser(User user)
        //{
        //    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
        //    user.Password = hashedPassword;
        //    return await _userManager.CreateAsync(user);
        //}

        //public async Task<User?> FindByEmailAsync(string email)
        //{
        //    return await _userRepository.Get(u => u.Email == email);
        //}



        public Task<User> CreateNewUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool CheckPassword(User user, string password)
        {
            throw new NotImplementedException();
        }

        public Task<User?> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
