using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IBookService
    {
        public Task<Book?> CreateBookAsync(Book book);
        public Task<IEnumerable<Book?>> GetAllBooksAsync(BookQueryObject bookQueryObject);
        public Task<Book?> GetBookByIdAsync(int bookId);
        public Task DeleteBookAsync(Book book);
        public Task<Book?> UpdateBookAsync(Book book);
    }
}
