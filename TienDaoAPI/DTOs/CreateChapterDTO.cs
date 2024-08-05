using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TienDaoAPI.DTOs
{
    public class CreateChapterDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Content { get; set; }
        [JsonIgnore]
        public int Index { get; set; }
        public int BookId { get; set; }
        [JsonIgnore]
        public int OwnerId { get; set; }
    }
}
