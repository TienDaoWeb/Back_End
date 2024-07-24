using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TienDaoAPI.DTOs;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public ProfileService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<bool> ChangeAvatarAsync(int userId, string avatarUrl)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                user!.AvatarUrl = avatarUrl;
                var identityResult = await _userManager.UpdateAsync(user);
                return identityResult.Succeeded;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> ChangeProfileAsync(int userId, UpdateProfileDTO dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                _mapper.Map(dto, user);
                var identityResult = await _userManager.UpdateAsync(user!);
                return identityResult.Succeeded;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
