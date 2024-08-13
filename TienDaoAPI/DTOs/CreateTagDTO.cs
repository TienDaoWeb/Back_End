using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class CreateTagDTO
    {
        [Required]
        public required string Name { get; set; } = string.Empty;
        [Required]
        public int TagTypeId { get; set; }
    }
}
