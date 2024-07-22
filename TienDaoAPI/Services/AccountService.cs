using Microsoft.AspNetCore.Identity;
using TienDaoAPI.Enums;
using TienDaoAPI.Helpers;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly EmailProvider _emailProvider;
        private readonly JwtHandler _jwtHandler;
        public AccountService(UserManager<User> userManager, EmailProvider emailProvider, JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _emailProvider = emailProvider;
            _jwtHandler = jwtHandler;
        }


        public async Task<AccountErrorEnum> CreateNewAccountAsync(User user, string password)
        {

            var identityResult = await _userManager.CreateAsync(user, password);

            if (identityResult.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var templatePath = "./Templates/register.html";
                await _emailProvider.SendEmailWithTemplateAsync(user.Email!, "Email Verification", templatePath, new { code });
                return AccountErrorEnum.AllOk;
            }
            return AccountErrorEnum.Existed;

        }

        public async Task<AccountErrorEnum> VerifyEmailAsync(string email, string code)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return AccountErrorEnum.NotExists;
            }
            var identityResult = await _userManager.ConfirmEmailAsync(user, code);
            if (identityResult.Succeeded)
            {
                return AccountErrorEnum.AllOk;
            }
            return AccountErrorEnum.InvalidOTP;
        }
    }
}
