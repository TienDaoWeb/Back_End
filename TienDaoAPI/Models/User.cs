using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
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
        [DefaultValue(GenderEnum.Private)]
        public GenderEnum Gender { get; set; } = GenderEnum.Private;
        [Required]
        [RoleEnumValidation]
        public string Role { get; set; } = RoleEnum.READER;
        public bool IsDisabled { get; set; } = false;
    }
}
