using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TienDaoAPI.Attributes;
using TienDaoAPI.DTOs;
using TienDaoAPI.Enums;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

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

                var book = await _bookService.CreateBookAsync(dto);
                if (book == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Không thể tạo sách mới. Vui lòng kiểm tra lại thông tin.",
                    });
                }
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Thêm truyện thành công!",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
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
                    return StatusCode(StatusCodes.Status404NotFound, new Response
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "Book does not exists"
                    });
                }

                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = _mapper.Map<BookDTO>(book)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Máy chủ đang gặp lỗi: " + ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        [Owner(typeof(Book))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _bookService.DeleteBookAsync(id);

                if (result)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Thành công",
                    });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Truyện không tồn tại hoặc đã bị xóa!",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Máy chủ đang gặp lỗi: " + ex.Message
                });
            }

        }

        [HttpPut]
        [Route("{id}")]
        [Authorize]
        [Owner(typeof(Book))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDTO dto)
        {
            try
            {
                dto.OwnerId = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Truyện không tồn tại",
                    });
                }
                var updatedBook = await _bookService.UpdateBookAsync(book, dto);

                if (updatedBook != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Cập nhật thông tin truyện thành công!",
                    });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Cập nhật thông tin truyện không thành công!",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Máy chủ đang gặp lỗi: " + ex.Message
                });
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks([FromQuery] BookFilter filter)
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync(filter);
                var count = books.Count();
                var paginatedBooks = books.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize);

                return StatusCode(StatusCodes.Status200OK, new PaginatedResponse
                {
                    PageNumber = filter.Page,
                    PageSize = filter.PageSize,
                    TotalItems = count,
                    TotalPages = (int)Math.Ceiling(count / (double)filter.PageSize),
                    Data = _mapper.Map<IEnumerable<BookDTO>>(paginatedBooks)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Máy chủ đang gặp lỗi: " + ex.Message
                });
            }
        }


    }
}
