using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TienDaoAPI.DTOs
{
    public class CreateReadingDTO
    {
        [Required]
        public int ChapterId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
