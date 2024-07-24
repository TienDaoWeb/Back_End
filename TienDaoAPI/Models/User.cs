using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TienDaoAPI.Enums;
using TienDaoAPI.Validation;

namespace TienDaoAPI.Models
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime? Birthday { get; set; }
        public string? AvatarUrl { get; set; }
        [Required]
        [RoleEnumValidation]
        public string Role { get; set; } = RoleEnum.ADMIN;
        public bool IsDisabled { get; set; } = false;
    }
}
