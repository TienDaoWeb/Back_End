using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TienDaoAPI.Data;
using TienDaoAPI.DTOs.Books;
using TienDaoAPI.DTOs.Users;
using TienDaoAPI.Helpers;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Services
{
    public class BookService : IBookService
    {
        private readonly TienDaoDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAuthorService _authorService;
        public BookService(IMapper mapper, IAuthorService authorService, TienDaoDbContext dbContext)
        {
            _mapper = mapper;
            _authorService = authorService;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateBookAsync(CreateBookDTO dto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var author = await _authorService.GetAuthorAsync(dto.Author.Name);
                if (author == null)
                {
                    author = await _authorService.CreateAuthorAsync(dto.Author);
                    if (author == null)
                    {
                        return false;
                    }
                }

                var book = _mapper.Map<Book>(dto);
                book.AuthorId = author.Id;
                var createdBook = _dbContext.Books.Add(book).Entity;
                await _dbContext.SaveChangesAsync();
                if (dto.Tags is not null)
                {
                    foreach (var id in dto.Tags)
                    {
                        var bookTag = new BookTag
                        {
                            BookId = createdBook.Id,
                            TagId = id
                        };
                        _dbContext.BookTags.Add(bookTag);
                    }
                }
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteBookAsync(Book book)
        {
            try
            {
                book.DeletedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Book?>> GetAllBooksAsync([FromQuery] BookFilter filter)
        {
            var filterExpression = ExpressionProvider<Book>.BuildBookFilter(filter);
            var sortExpression = ExpressionProvider<Book>.GetSortExpression(filter.SortBy);
            var books = _dbContext.Books
                .Include(b => b.User)
                .Include(b => b.Comments)
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Chapters)
                .Include(b => b.Comments)
                .Include(b => b.Reviews)
                .Include(b => b.BookTags)
                .ThenInclude(bt => bt.Tag)
                .Where(filterExpression);
            books = filter.SortBy != null && filter.SortBy.StartsWith("-")
            ? books.OrderByDescending(sortExpression)
            : books.OrderBy(sortExpression);
            return await books.ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _dbContext.Books
                .Include(b => b.User)
                .Include(b => b.Comments)
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Chapters)
                .Include(b => b.Comments)
                .Include(b => b.Reviews)
                .Include(b => b.BookTags)
                .ThenInclude(bt => bt.Tag)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateBookAsync(Book book, UpdateBookDTO dto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var author = await _authorService.GetAuthorAsync(dto.Author.Name);
                if (author == null)
                {
                    author = await _authorService.CreateAuthorAsync(dto.Author);
                    if (author == null)
                    {
                        return false;
                    }
                    book.AuthorId = author.Id;
                }

                _mapper.Map(dto, book);
                book.UpdatedAt = DateTime.UtcNow;


                var existingTagIds = book.BookTags.Select(bt => bt.TagId).ToList();
                var newTagIds = dto.TagIds;

                foreach (var existingTagId in existingTagIds)
                {
                    if (!newTagIds.Contains(existingTagId))
                    {
                        var tagToRemove = book.BookTags.First(bt => bt.TagId == existingTagId);
                        _dbContext.BookTags.Remove(tagToRemove);
                    }
                }

                foreach (var newTagId in newTagIds)
                {
                    if (!existingTagIds.Contains(newTagId))
                    {
                        var newBookTag = new BookTag { BookId = book.Id, TagId = newTagId };
                        _dbContext.BookTags.Add(newBookTag);
                    }
                }

                _dbContext.Books.Update(book);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public async Task<bool> ChangePosterAsync(Book book, string posterUrl)
        {
            try
            {
                book.PosterUrl = posterUrl;
                _dbContext.Books.Update(book);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Modifiable(Book book, UserDTO user)
        {
            return book.OwnerId == user.Id;
        }
    }
}
