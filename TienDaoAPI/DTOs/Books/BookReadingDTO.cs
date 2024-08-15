using TienDaoAPI.Models;

namespace TienDaoAPI.DTOs.Books
{
    public class BookReadingDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public Author? Author { get; set; }

        public string? PosterUrl { get; set; }

        public int LastestIndex { get; set; } = 0;
    }
}
