using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TienDaoAPI.DTOs
{
    public class CreateReplyCommentDTO
    {
        [Required]
        public required string Content { get; set; }
        [Required]
        public int CommentParentId { get; set; }
        [JsonIgnore]
        public int OwnerId { get; set; }
    }
}
