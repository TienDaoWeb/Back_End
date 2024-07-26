using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class OTPEmailDTO : EmailDTO
    {
        [Required]
        public string OTP { get; set; } = string.Empty;
    }
}
