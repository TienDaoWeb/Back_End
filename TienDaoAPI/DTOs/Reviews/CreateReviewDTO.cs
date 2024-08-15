using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TienDaoAPI.DTOs.Reviews
{
    public class CreateReviewDTO
    {
        [Required]
        [Range(0, 5)]
        public float Score { get; set; }

        public string? Content { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int IdReadChapter { get; set; }

        [JsonIgnore]
        public int OwnerId { get; set; }
    }
}
