using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Tags
{
    public class CreateTagDTO
    {
        [Required]
        public required string Name { get; set; } = string.Empty;
        [Required]
        public int TagTypeId { get; set; }
    }
}
