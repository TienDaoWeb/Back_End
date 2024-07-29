using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class CreateChapterDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Content { get; set; }
        public int BookId { get; set; }
    }
}
