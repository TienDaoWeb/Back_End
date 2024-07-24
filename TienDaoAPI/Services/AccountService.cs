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
                var otp = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var templatePath = "./Templates/otp_register_mail.html";
                await _emailProvider.SendEmailWithTemplateAsync(user.Email!, "OTP Verification", templatePath, new { otp });
                return AccountErrorEnum.AllOk;
            }
            return AccountErrorEnum.Existed;

        }

        public async Task<AccountErrorEnum> RequestResetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var otp = await _userManager.GeneratePasswordResetTokenAsync(user);
                var templatePath = "./Templates/otp_reset_password.html";
                await _emailProvider.SendEmailWithTemplateAsync(user.Email!, "OTP Verification", templatePath, new { otp });
                return AccountErrorEnum.AllOk;
            }
            return AccountErrorEnum.NotExists;
        }

        public async Task<AccountErrorEnum> ResetPasswordAsync(string email, string otp)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return AccountErrorEnum.NotExists;
            }
            var newPassword = PasswordProvider.GenerateStrongPassword();
            var identityResult = await _userManager.ResetPasswordAsync(user, otp, newPassword);
            if (!identityResult.Succeeded)
            {
                return AccountErrorEnum.InvalidOTP;
            }
            var templatePath = "./Templates/reset_password_mail.html";
            await _emailProvider.SendEmailWithTemplateAsync(user.Email!, "New your password", templatePath, new { newPassword });
            return AccountErrorEnum.AllOk;
        }

        public async Task<AccountErrorEnum> VerifyEmailAsync(string email, string otp)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return AccountErrorEnum.NotExists;
            }
            var identityResult = await _userManager.ConfirmEmailAsync(user, otp);
            if (!identityResult.Succeeded)
            {
                return AccountErrorEnum.InvalidOTP;
            }
            return AccountErrorEnum.AllOk;
        }

        public async Task<AccountErrorEnum> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return AccountErrorEnum.NotExists;
            }
            var identityResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!identityResult.Succeeded)
            {
                return AccountErrorEnum.IncorrectPassword;
            }
            if (!PasswordProvider.CheckPasswordStrength(newPassword))
            {
                return AccountErrorEnum.WeakPassword;
            }
            return AccountErrorEnum.AllOk;
        }
    }
}
