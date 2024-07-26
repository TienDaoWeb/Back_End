using Microsoft.AspNetCore.Mvc;
using System.Net;
using TienDaoAPI.DTOs;
using TienDaoAPI.Utils;

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
        public async Task<IActionResult> UploadImageAsync([FromForm] ImageDTO dto)
        {
            try
            {
                var url = await _firebaseStorageService.UploadImageAsync(dto.Image);

                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new { Url = url }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Internal Server Error: " + ex.Message
                });
            }
        }
    }
}
