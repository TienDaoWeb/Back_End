using TienDaoAPI.DTOs;

namespace TienDaoAPI.Services.IServices
{
    public interface IProfileService
    {
        public Task<bool> ChangeAvatarAsync(int userId, string avatarUrl);
        public Task<bool> ChangeProfileAsync(int userId, UpdateProfileDTO dto);
    }
}
