using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TienDaoAPI.DTOs;
using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.DTOs.Responses;
using TienDaoAPI.Enums;
using TienDaoAPI.Models;
using TienDaoAPI.Response;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IImageStorageService _firebaseStorageService;
        private readonly IBookService _bookService;
        private readonly IChapterService _chapterService;
        private readonly IMapper _mapper;
        private readonly IGenreService _genreService;
        private readonly IUserService _userService;
        private readonly IAuthorService _authorService;

        public BookController(IImageStorageService firebaseStorageService, IBookService bookService,
            IMapper mapper, IGenreService genreService, IUserService userService, IChapterService chapterService, IAuthorService authorService)
        {
            _firebaseStorageService = firebaseStorageService;
            _bookService = bookService;
            _chapterService = chapterService;
            _mapper = mapper;
            _genreService = genreService;
            _userService = userService;
            _authorService = authorService;
        }

        [HttpPost]
        [Authorize(Roles = RoleEnum.CONVERTER)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateBookDTO dto)
        {
            try
            {
                dto.OwnerId = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;
                var author = await _authorService.GetAuthorAsync(dto.Author.Name);
                if (author == null)
                {
                    author = await _authorService.CreateAuthorAsync(dto.Author);
                }
                var book = _mapper.Map<Book>(dto);
                book.AuthorId = author.Id;
                var newBook = await _bookService.CreateBookAsync(book);
                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Thêm truyện thành công!",
                    Result = newBook
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Máy chủ đang gặp lỗi: " + ex.Message
                });
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "Book does not exists"
                    });
                }

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<BookResponse>(book)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Máy chủ đang gặp lỗi: " + ex.Message
                });
            }
        }
        // Delete book 
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Book does not exist",

                    });
                }
                if (book.PosterUrl != null)
                {
                    //var storage = StorageClient.Create();
                    //// Xóa file ảnh
                    //await storage.DeleteObjectAsync("tiendaoapi.appspot.com", $"images/{book.PosterUrl}");
                }
                //Xóa tất cả các chapter của book
                var listChapter = await _chapterService.GetAllChapterOfBookAsync(chapter => chapter.BookId == book.Id);
                if (listChapter != null)
                {
                    var nonNullChapters = listChapter.Where(chapter => chapter != null).Cast<Chapter>();
                    await _chapterService.DeleteAllChapterAsync(nonNullChapters);
                }

                // Xóa Book 
                await _bookService.DeleteBookAsync(book);

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Delete book successfully",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Máy chủ đang gặp lỗi: " + ex.Message
                });
            }

        }
        //Update Book
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BookUpdate(int id, [FromForm] CreateBookDTO newBook)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Can't find book",
                    });
                }
                else
                {
                    //book.Author = newBook.Author;
                    book.Synopsis = newBook.Synopsis;


                    await _bookService.UpdateBookAsync(book);

                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Update book successfully",
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Máy chủ đang gặp lỗi: " + ex.Message
                });
            }
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks([FromQuery] BookQueryObject bookQueryObject)
        {
            try
            {
                var stories = await _bookService.GetAllBooksAsync(bookQueryObject);

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Result ",
                    Result = _mapper.Map<IEnumerable<BookResponse>>(stories)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Máy chủ đang gặp lỗi: " + ex.Message
                });
            }
        }


    }
}
