using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Genres
{
    public class CreateGenreDTO
    {
        [Required]
        public required string Name { get; set; } = string.Empty;
        [Required]
        public required string Description { get; set; } = string.Empty;
    }
}
