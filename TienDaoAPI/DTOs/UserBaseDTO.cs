using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs
{
    public class UserBaseDTO
    {
        public int Id { get; set; }

        [Required]
        public required string FullName { get; set; }
    }
}
