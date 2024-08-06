using TienDaoAPI.Models;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Services.IServices
{
    public interface IUserService
    {
        public Task<User?> GetUserByIdAsync(int id);
        public Task<IEnumerable<User?>> GetAllUsers(UserFilter filter);
    }
}
