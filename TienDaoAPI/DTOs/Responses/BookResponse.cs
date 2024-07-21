namespace TienDaoAPI.DTOs.Responses
{
    public class BookResponse
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Author { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public int NumberOfChapters { get; set; }

        public GenreResponse? GenreResponse { get; set; }
    }
}
