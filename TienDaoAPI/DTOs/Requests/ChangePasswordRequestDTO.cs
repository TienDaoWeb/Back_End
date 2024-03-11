using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Requests
{
    public class ChangePasswordRequestDTO
    {
        [DataType(DataType.Password)]
        public required string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        [Required, MinLength(6)]
        public required string NewPassword { get; set; }
        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        public required string ConfirmNewPassword { get; set; }
    }
}
