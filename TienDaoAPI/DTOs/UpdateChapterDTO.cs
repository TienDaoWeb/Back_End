using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class UpdateChapterDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public int Index { get; set; }
    }
}
