using TienDaoAPI.Enums;

namespace TienDaoAPI.DTOs
{
    public class UserDTO : UserBaseDTO
    {
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? Birthday { get; set; }

        public string? AvatarUrl { get; set; }

        public GenderEnum Gender { get; set; }

        public required string Role { get; set; }
    }
}
