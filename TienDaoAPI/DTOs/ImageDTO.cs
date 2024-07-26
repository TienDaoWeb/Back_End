using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class ImageDTO
    {
        [Required]
        public required IFormFile Image { get; set; }
    }
}
