using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TienDaoAPI.DTOs.Bookmarks
{
    public class CreateBookmarkDTO
    {
        [Required]
        public int BookId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
