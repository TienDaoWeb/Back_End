using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<RefreshToken?> createRefreshTokenAsync(string email)
        {
            var user = await _userRepository.GetAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }

            var refreshToken = new RefreshToken
            {
                Token = new Guid(),
                ExpiredAt = DateTime.Now.AddDays(1),
                UserId = user.Id
            };

            refreshToken = await _refreshTokenRepository.CreateAsync(refreshToken);
            return refreshToken;
        }
    }
}
