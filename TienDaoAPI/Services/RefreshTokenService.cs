using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.UnitOfWork;

namespace TienDaoAPI.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RefreshToken?> createRefreshTokenAsync(string email)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.Email == email);
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

            refreshToken = _unitOfWork.RefreshTokenRepository.Create(refreshToken);
            await _unitOfWork.SaveAsync();
            return refreshToken;
        }
    }
}
