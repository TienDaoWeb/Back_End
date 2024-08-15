using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TienDaoAPI.DTOs.Comments
{
    public class CreateCommentDTO
    {
        [Required]
        public required string Content { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int ChapterId { get; set; }
        [JsonIgnore]
        public int OwnerId { get; set; }
    }
}
