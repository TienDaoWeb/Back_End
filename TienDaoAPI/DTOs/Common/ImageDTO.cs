using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Common
{
    public class ImageDTO
    {
        [Required]
        public required IFormFile Image { get; set; }
    }
}
