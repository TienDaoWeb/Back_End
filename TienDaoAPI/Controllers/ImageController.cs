using Microsoft.AspNetCore.Mvc;
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

                return Ok(new Response().Success().SetData(new { Url = url }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }
    }
}
