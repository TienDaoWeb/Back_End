using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IRefreshTokenService
    {
        public Task<RefreshToken?> createRefreshTokenAsync(string email);



    }
}
