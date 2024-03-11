using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IRefreshTokenService
    {
        public Task<RefreshToken> createRefreshTokenAsync(User user);
        public Task<RefreshToken?> getRefreshTokenByTokenAsync(Guid token);
        public Task<bool> CheckTokenExpiredAsync(RefreshToken refreshToken);

    }
}
