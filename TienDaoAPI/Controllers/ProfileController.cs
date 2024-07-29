using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TienDaoAPI.DTOs;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IImageStorageService _imageStorageService;
        private readonly IProfileService _profileService;
        public ProfileController(IImageStorageService imageStorageService, IProfileService profileService)
        {
            _imageStorageService = imageStorageService;
            _profileService = profileService;
        }


        [HttpPost]
        [Route("change-avatar")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeAvatar([FromForm] ImageDTO dto)
        {
            try
            {
                var userId = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;
                var url = await _imageStorageService.UploadImageAsync(dto.Image);
                var result = await _profileService.ChangeAvatarAsync(userId, url);
                if (result)
                {
                    return Ok(new Response().Success().SetMessage("Thành công!"));
                }
                return BadRequest(new Response().BadRequest().SetMessage("Có chút trục trặc trong khi chúng tôi đang cố gắng thay bức hình tuyệt vời này!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }

        [HttpPatch]
        [Route("me")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeProfile([FromBody] UpdateProfileDTO dto)
        {
            try
            {
                var userId = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;
                var result = await _profileService.ChangeProfileAsync(userId, dto);
                if (result)
                {
                    return Ok(new Response().Success().SetMessage("Cập nhật thông tin cá nhân thành công!"));
                }
                return BadRequest(new Response().BadRequest().SetMessage("Có chút trục trặc trong khi chúng tôi thông tin cá nhân của bạn!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }
    }
}
