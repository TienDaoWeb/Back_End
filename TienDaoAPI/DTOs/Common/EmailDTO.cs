using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Common
{
    public class EmailDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
    }
}
