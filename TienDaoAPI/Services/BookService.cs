using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using TienDaoAPI.DTOs;
using TienDaoAPI.Helpers;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorService _authorService;
        public BookService(IBookRepository bookRepository, IMapper mapper, IAuthorService authorService)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _authorService = authorService;
        }

        public async Task<Book?> CreateBookAsync(CreateBookDTO dto)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var author = await _authorService.GetAuthorAsync(dto.Author.Name);
                    if (author == null)
                    {
                        author = await _authorService.CreateAuthorAsync(dto.Author);
                        if (author == null)
                        {
                            return null;
                        }
                    }

                    var book = _mapper.Map<Book>(dto);
                    book.AuthorId = author.Id;

                    var createdBook = await _bookRepository.CreateAsync(book);
                    if (createdBook != null)
                    {
                        scope.Complete();
                    }
                    return createdBook;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteBookAsync(Book book)
        {
            try
            {
                book.DeletedAt = DateTime.UtcNow;
                await _bookRepository.SaveAsync();
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
            var sortExpression = filter.SortBy == null ? null : ExpressionProvider<Book>.GetSortExpression(filter.SortBy);
            filter.Include += "Chapters,Comments,Reviews";
            var books = await _bookRepository.FilterAsync(filterExpression, filter.Include, sortExpression);

            return books;
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetAsync(b => b.Id == id, "User,Author,Genre,Chapters,Comments,Reviews");
        }

        public async Task<Book?> UpdateBookAsync(Book book, UpdateBookDTO dto)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var author = await _authorService.GetAuthorAsync(dto.Author.Name);
                    if (author == null)
                    {
                        author = await _authorService.CreateAuthorAsync(dto.Author);
                        if (author == null)
                        {
                            return null;
                        }
                        book.AuthorId = author.Id;
                    }

                    _mapper.Map(dto, book);
                    book.UpdatedAt = DateTime.UtcNow;

                    var updatedBook = await _bookRepository.UpdateAsync(book);
                    if (updatedBook != null)
                    {
                        scope.Complete();
                    }
                    return updatedBook;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public async Task<bool> ChangePosterAsync(Book book, string posterUrl)
        {
            try
            {
                book.PosterUrl = posterUrl;
                await _bookRepository.SaveAsync();
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
