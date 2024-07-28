using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using TienDaoAPI.DTOs;
using TienDaoAPI.Helpers;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Services.IServices;

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

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book != null)
            {
                book.DeletedAt = DateTime.UtcNow;
                await _bookRepository.SaveAsync();
                return true;
            }
            return false;

        }

        public async Task<IEnumerable<Book?>> GetAllBooksAsync([FromQuery] BookFilter filter)
        {
            var filterExpression = ExpressionProvider<Book>.BuildBookFilter(filter);
            var sortExpression = filter.SortBy == null ? null : ExpressionProvider<Book>.GetSortExpression(filter.SortBy);
            var books = await _bookRepository.FilterAsync(filterExpression, filter.Include, sortExpression);

            return books;
        }

        public async Task<Book?> GetBookByIdAsync(int bookId)
        {
            return await _bookRepository.GetByIdAsync(bookId);
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
                    }

                    _mapper.Map(dto, book);
                    book.AuthorId = author.Id;
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


    }
}
