using Microsoft.AspNetCore.Mvc;
using System.Net;
using TienDaoAPI.DTOs;
using TienDaoAPI.Response;

namespace TienDaoAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageStorageService _firebaseStorageService;
        public ImageController(IImageStorageService firebaseStorageService)
        {
            _firebaseStorageService = firebaseStorageService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImageAsync([FromForm] UploadImageDTO dto)
        {
            try
            {
                var url = await _firebaseStorageService.UploadImageAsync(dto.Image);

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = new { Url = url }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Internal Server Error: " + ex.Message
                });
            }
        }
    }
}
