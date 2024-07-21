using Microsoft.AspNetCore.Identity;

namespace TienDaoAPI.Models
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime? Birthday { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
