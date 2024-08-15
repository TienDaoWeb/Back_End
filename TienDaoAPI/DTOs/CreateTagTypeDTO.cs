using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class CreateTagTypeDTO
    {
        [Required]
        public required string Name { get; set; } = string.Empty;
    }
}
