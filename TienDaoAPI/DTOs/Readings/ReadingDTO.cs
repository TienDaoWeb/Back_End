using TienDaoAPI.DTOs.Books;

namespace TienDaoAPI.DTOs.Readings
{
    public class ReadingDTO
    {
        public int Id { get; set; }

        public int ChapterId { get; set; }

        public int ChapterIndex { get; set; }

        public int BookId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public BookReadingDTO? Book { get; set; }
    }
}
