using TienDaoAPI.DTOs;
using TienDaoAPI.Models;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Services.IServices
{
    public interface IBookService
    {
        public Task<bool> CreateBookAsync(CreateBookDTO dto);
        public Task<IEnumerable<Book?>> GetAllBooksAsync(BookFilter filter);
        public Task<Book?> GetBookByIdAsync(int bookId);
        public Task<bool> DeleteBookAsync(Book book);
        public Task<bool> UpdateBookAsync(Book book, UpdateBookDTO dto);
        public Task<bool> ChangePosterAsync(Book book, string posterUrl);
        public bool Modifiable(Book book, UserDTO user);
    }
}
