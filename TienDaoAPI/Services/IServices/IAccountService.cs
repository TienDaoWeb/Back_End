using TienDaoAPI.Enums;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IAccountService
    {
        //public Task<AccountErrorEnum> SignUpAsync(string email, string password);
        public Task<AccountErrorEnum> CreateNewAccountAsync(User user, string password);
        public Task<AccountErrorEnum> VerifyEmailAsync(string email, string otp);
        public Task<AccountErrorEnum> RequestResetPasswordAsync(string email);
        public Task<AccountErrorEnum> ResetPasswordAsync(string email, string otp);
    }
}
