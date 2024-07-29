using TienDaoAPI.Enums;
using TienDaoAPI.Models;

namespace TienDaoAPI.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Synopsis { get; set; }

        public Author? Author { get; set; }

        public string? PosterUrl { get; set; }

        public int ReviewCount { get; set; } = 0;

        public int BookmarkCount { get; set; } = 0;

        public int ViewCount { get; set; } = 0;

        public int VoteCount { get; set; } = 0;

        public int WordCount { get; set; } = 0;

        public int LastestIndex { get; set; } = 0;

        public BookStatusEnum Status { get; set; }

        public string? StatusName { get; set; }

        public DateTime? PublishedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Genre? Genre { get; set; }

        public UserBaseDTO? Owner { get; set; }
    }
}
