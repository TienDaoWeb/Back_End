using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.TagTypes
{
    public class CreateTagTypeDTO
    {
        [Required]
        public required string Name { get; set; } = string.Empty;
    }
}
