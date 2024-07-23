using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class EmailDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
    }
}
