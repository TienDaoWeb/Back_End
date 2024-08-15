using System.ComponentModel.DataAnnotations;
using TienDaoAPI.Enums;

namespace TienDaoAPI.DTOs.Users
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required]
        public required string FullName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? Birthday { get; set; }

        public string? AvatarUrl { get; set; }

        public GenderEnum Gender { get; set; }

        public required string Role { get; set; }

        public required string Status { get; set; }
    }
}
