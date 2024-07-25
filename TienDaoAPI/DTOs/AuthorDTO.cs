using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class AuthorDTO
    {
        [Required]
        public required string Name { get; set; }
    }
}
