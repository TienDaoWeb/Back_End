using System.ComponentModel.DataAnnotations;
using TienDaoAPI.DTOs.Common;

namespace TienDaoAPI.DTOs.Users
{
    public class OTPEmailDTO : EmailDTO
    {
        [Required]
        public string OTP { get; set; } = string.Empty;
    }
}
