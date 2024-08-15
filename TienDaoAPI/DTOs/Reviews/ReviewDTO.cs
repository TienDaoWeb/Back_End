using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TienDaoAPI.Models;

namespace TienDaoAPI.DTOs.Reviews
{
    public class ReviewDTO
    {
        [Range(0, 5)]
        public float Score { get; set; } = 5;

        public string? Content { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<int>? UsersReaction { get; set; } = [];

        public int BookId { get; set; } = 0;
        public int IdReadChapter { get; set; } = 0;
        public int? OwnerId { get; set; }

    }
}
