namespace TienDaoAPI.DTOs
{
    public class ChapterInfoDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? Index { get; set; }

        public int ViewCount { get; set; }

        public int WordCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? PublishedAt { get; set; }

        public int OwnerId { get; set; }

        public int BookId { get; set; }
    }
}
