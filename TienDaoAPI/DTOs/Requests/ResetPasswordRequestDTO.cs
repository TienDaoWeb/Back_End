using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Requests
{
    public class ResetPasswordRequestDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required, MinLength(6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;
        [Required, Compare("NewPassword")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
