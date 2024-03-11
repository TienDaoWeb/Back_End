using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IUserService
    {
        public Task<User> CreateNewUser(User user);
        public Task<User?> FindByEmailAsync(string email);
        public bool CheckPassword(User user, string password);
        public Task<User?> GetUserByIdAsync(int id);
    }
}
