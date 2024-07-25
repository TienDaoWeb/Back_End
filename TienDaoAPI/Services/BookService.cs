using System.Linq.Expressions;
using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book?> CreateBookAsync(CreateBookDto dto)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.PosterUrl;
            //await _firebaseStorageService.UploadImageAsync(uniqueFileName, dto.PosterUrl!);

            Book newBook = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Description = dto.Description,
                PosterUrl = uniqueFileName,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                OwnerId = dto.OwnerId,
                GenreId = dto.GenreId
            };

            return await _bookRepository.CreateAsync(newBook);
        }

        public async Task DeleteBookAsync(Book book)
        {
            await _bookRepository.DeleteAsync(book);

        }

        public async Task<IEnumerable<Book?>> GetAllBooksAsync(BookQueryObject queryObj)
        {
            var query = (await _bookRepository.GetAllAsync()).AsQueryable();
            if (!string.IsNullOrEmpty(queryObj.Keyword))
            {
                query = query.Where(s => s.Title.Contains(queryObj.Keyword));
            }
            if (queryObj.Genre > 0)
            {
                query = query.Where(s => s.GenreId == queryObj.Genre);
            }
            var today = DateTime.Today;
            var columnsMap = new Dictionary<string, Expression<Func<Book, object>>>
            {
                //["view_day"] = s => s.bookAudits.First(sw => sw.ViewDate.Date == today.Date),
                //["view_month"] = s => s.bookAudits.Sum(sw => sw.ViewCount * (sw.ViewDate.Month == today.Month && sw.ViewDate.Year == today.Year ? 1 : 0)),
                //["view_week"] = s => s.bookAudits.Sum(sw => sw.ViewCount * (sw.ViewDate >= today.AddDays(-(int)today.DayOfWeek) && sw.ViewDate < today.AddDays(7 - (int)today.DayOfWeek) ? 1 : 0)),
                ["view_count"] = s => s.ViewCount,
                ["create_at"] = s => s.CreateDate,
                ["update_at"] = s => s.UpdateDate,
                ["chapter_count"] = s => s.Chapters.Count,
                ["review_count"] = s => s.Reviews.Count,
                ["comment_count"] = s => s.Comments.Count
            };
            if (!string.IsNullOrEmpty(queryObj.SortBy) && columnsMap.ContainsKey(queryObj.SortBy))
            {
                query = queryObj.IsSortAscending ? query.OrderBy(columnsMap[queryObj.SortBy]) : query.OrderByDescending(columnsMap[queryObj.SortBy]);
            }
            if (queryObj.Page <= 0)
            {
                queryObj.Page = 1;
            }
            if (queryObj.PageSize > 20 || queryObj.PageSize <= 0)
            {
                queryObj.PageSize = 20;
                query = query.Skip(queryObj.PageSize * (queryObj.Page - 1)).Take(queryObj.PageSize);
            }

            return query;
        }

        public async Task<Book?> GetBookByIdAsync(int bookId)
        {
            return await _bookRepository.GetByIdAsync(bookId);
        }

        public async Task<Book?> UpdateBookAsync(Book book)
        {
            return await _bookRepository.UpdateAsync(book);
        }


    }
}
