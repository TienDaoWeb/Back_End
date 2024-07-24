using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TienDaoAPI.DTOs;
using TienDaoAPI.Response;
using TienDaoAPI.Services.IServices;

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
        public async Task<IActionResult> ChangeAvatar([FromForm] UploadImageDTO dto)
        {
            try
            {
                var userId = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;
                var url = await _imageStorageService.UploadImageAsync(dto.Image);
                var result = await _profileService.ChangeAvatarAsync(userId, url);
                if (result)
                {
                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Cập nhật ảnh đại diện thành công",
                        Result = new { Url = url }
                    });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Có chút trục trặc trong khi chúng tôi đang cố gắng thay bức hình tuyệt vời này!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Internal Server Error: " + ex.Message,

                });
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
                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Cập nhật thông tin cá nhân thành công"
                    });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Có chút trục trặc trong khi chúng tôi đang cố gắng thay bức hình tuyệt vời này!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Internal Server Error: " + ex.Message,

                });
            }
        }
    }
}
