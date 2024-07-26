using TienDaoAPI.DTOs;
using TienDaoAPI.Models;

namespace TienDaoAPI.Services.IServices
{
    public interface IBookService
    {
        public Task<Book?> CreateBookAsync(CreateBookDTO dto);
        public Task<IEnumerable<Book?>> GetAllBooksAsync(BookFilter filter);
        public Task<Book?> GetBookByIdAsync(int bookId);
        public Task<bool> DeleteBookAsync(int id);
        public Task<Book?> UpdateBookAsync(Book book);
    }
}
