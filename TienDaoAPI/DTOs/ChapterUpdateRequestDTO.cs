using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class ChapterUpdateRequestDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Content { get; set; }
    }
}
