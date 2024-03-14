using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TienDaoAPI.Models;

namespace TienDaoAPI.DTOs.Requests
{
    public class ChapterPostRequestDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Content { get; set; }
        public int StoryId { get; set; }
    }
}
