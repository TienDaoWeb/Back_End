using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Helpers
{
    public class CustomTwoFactorTokenProvider : IUserTwoFactorTokenProvider<User>
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly CustomTwoFactorTokenProviderOptions _options;

        public CustomTwoFactorTokenProvider(IRedisCacheService tokenStoreService, IOptions<CustomTwoFactorTokenProviderOptions> options)
        {
            _redisCacheService = tokenStoreService;
            _options = options.Value;
        }
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
        {
            return Task.FromResult(manager.SupportsUserTwoFactor);
        }

        public Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user)
        {
            Random random = new Random();
            int token = random.Next(100000, 999999);


            if (_redisCacheService.Cache($"{purpose}:{user.Email}", token.ToString(), TimeSpan.FromMinutes(15)))
                return Task.FromResult(token.ToString());

            return Task.FromResult(string.Empty);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<User> manager, User user)
        {
            var storedToken = _redisCacheService.Get($"{purpose}:{user.Email}");
            if (storedToken != null)
            {
                bool result = storedToken.Equals(token);
                _redisCacheService.Remove($"{purpose}:{user.Email}");

                return Task.FromResult(result);
            }
            return Task.FromResult(false);
        }
    }
}

