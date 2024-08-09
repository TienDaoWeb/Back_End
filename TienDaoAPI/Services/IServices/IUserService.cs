using TienDaoAPI.Models;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Services.IServices
{
    public interface IUserService
    {
        public Task<User?> GetUserByIdAsync(int id);
        public Task<IEnumerable<User?>> GetAllUsers(UserFilter filter);
        ///<summary>
        ///Return value meaning:<br/>
        ///    - 1: Account blocked, cann't execute block operation.<br/>
        ///    - 2: Success.<br/>
        ///    - 3: Failed.
        ///</summary>
        public Task<int> LockUser(User user);
        public Task<bool> UnlockUser(User user);
    }
}
