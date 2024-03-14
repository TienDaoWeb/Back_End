using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Requests
{

    public class StoryRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Image { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public int UserId { get; set; }
        public required IFormFile UrlImage { get; set; }

    }
}
