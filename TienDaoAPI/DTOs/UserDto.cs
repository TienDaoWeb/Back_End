using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        [Required]
        public required string FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? Birthday { get; set; }

        public string? AvatarUrl { get; set; }


    }
}
