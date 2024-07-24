using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class UploadImageDTO
    {
        [Required]
        public required IFormFile Image { get; set; }
    }
}
