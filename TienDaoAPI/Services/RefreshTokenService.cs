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

        public async Task<RefreshToken> createRefreshTokenAsync(User user)
        {

            var refreshToken = await _refreshTokenRepository.GetAsync(u => u.UserId == user.Id);
            if (refreshToken != null)
            {
                refreshToken.Token = Guid.NewGuid();
                refreshToken.ExpiredAt = DateTime.UtcNow.AddDays(1);
                refreshToken = await _refreshTokenRepository.UpdateAsync(refreshToken);
            }
            else
            {
                var newRefreshToken = new RefreshToken
                {
                    Token = Guid.NewGuid(),
                    ExpiredAt = DateTime.UtcNow.AddDays(1),
                    UserId = user.Id
                };
                refreshToken = await _refreshTokenRepository.CreateAsync(newRefreshToken);
            }
            if (refreshToken == null)
            {
                throw new Exception("Can't create refresh token");
            }
            return refreshToken;
        }

        public async Task<RefreshToken?> getRefreshTokenByTokenAsync(Guid token)
        {
            return await _refreshTokenRepository.GetAsync(o => o.Token == token);
        }

        public async Task<bool> CheckTokenExpiredAsync(RefreshToken refreshToken)
        {
            if (DateTime.Compare(refreshToken.ExpiredAt, DateTime.Now) < 0)
            {
                await _refreshTokenRepository.DeleteAsync(refreshToken);
                return true;
            }
            return false;
        }
    }
}
