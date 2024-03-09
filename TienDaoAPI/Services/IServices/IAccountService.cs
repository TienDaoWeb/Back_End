using Microsoft.AspNetCore.Identity;

namespace TienDaoAPI.Services.IServices
{
    public interface IAccountService
    {
        public Task<IdentityResult> SignUpAsync(string username, string password);
        public Task<IdentityResult> SignInAsync(string username, string password);
    }
}
