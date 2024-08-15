using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Authors
{
    public class AuthorDTO
    {
        [Required]
        public required string Name { get; set; }
    }
}
