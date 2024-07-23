using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Requests
{
    public class ResetPasswordDTO : EmailDTO
    {
        [Required]
        public string OTP { get; set; } = string.Empty;
    }
}
