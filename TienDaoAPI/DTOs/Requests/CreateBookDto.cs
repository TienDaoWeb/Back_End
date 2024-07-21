using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Requests
{

    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Author { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;

        public IFormFile? PosterUrl { get; set; }

        [Required]
        public int OwnerId { get; set; }
        [Required]
        public int GenreId { get; set; }
        public required IFormFile UrlImage { get; set; }

    }
}
