using System.ComponentModel.DataAnnotations;
using TienDaoAPI.DTOs.Authors;

namespace TienDaoAPI.DTOs.Books
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

        public List<int> TagIds { get; set; } = new List<int>();
    }
}
