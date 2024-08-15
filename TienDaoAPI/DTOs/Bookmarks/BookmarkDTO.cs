using TienDaoAPI.DTOs.Books;

namespace TienDaoAPI.DTOs.Bookmarks
{
    public class BookmarkDTO
    {
        public int Id { get; set; }

        public BookBookmarkDTO? Book { get; set; }
    }
}
