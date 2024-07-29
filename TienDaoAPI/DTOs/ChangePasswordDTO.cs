using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public required string OldPassword { get; set; }
        [Required]
        public required string NewPassword { get; set; }
    }
}
