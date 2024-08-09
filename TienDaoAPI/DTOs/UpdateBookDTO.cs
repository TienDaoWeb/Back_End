using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class UpdateBookDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public required AuthorDTO Author { get; set; }
        [Required]
        public string Synopsis { get; set; } = string.Empty;
        [Required]
        public int GenreId { get; set; }
    }
}
