using Microsoft.AspNetCore.Identity;
using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<User> userManager, JwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityResult> SignInAsync(RegisterRequestDTO registerRequestDTO)
        {

            throw new NotImplementedException();
        }

        public Task<IdentityResult> SignInAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> SignUpAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
