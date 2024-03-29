using System.ComponentModel.DataAnnotations;

namespace TienDaoAPI.DTOs.Requests
{
    public class EmojiPostRequestDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ChapterId { get; set; }
        [Range(1, 6)]
        public int TypeEmoji { get; set; }
    }
}
