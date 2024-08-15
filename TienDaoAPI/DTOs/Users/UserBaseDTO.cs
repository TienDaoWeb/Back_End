using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Users
{
    public class UserBaseDTO
    {
        public int Id { get; set; }

        [Required]
        public required string FullName { get; set; }
    }
}
