using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TienDaoAPI.DTOs.Books;
using TienDaoAPI.DTOs.Chapters;
using TienDaoAPI.DTOs.Comments;
using TienDaoAPI.DTOs.Common;
using TienDaoAPI.DTOs.Reviews;
using TienDaoAPI.DTOs.Users;
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
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly IImageStorageService _imageStorageService;
        private readonly IChapterService _chapterService;
        private readonly ICommentService _commentService;
        private readonly IReviewService _reviewService;

        public BookController(IBookService bookService, IMapper mapper, IImageStorageService imageStorageService,
            IChapterService chapterService, ICommentService commentService, IReviewService reviewService)
        {
            _bookService = bookService;
            _mapper = mapper;
            _imageStorageService = imageStorageService;
            _chapterService = chapterService;
            _commentService = commentService;
            _reviewService = reviewService;
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
                if (!book)
                {
                    return BadRequest(new Response().BadRequest().SetMessage("Không thể tạo sách mới. Vui lòng kiểm tra lại thông tin."));
                }
                return Ok(new Response().Success().SetMessage("Thêm truyện thành công!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
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
                if (book is null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Truyện không tồn tại trong hệ thống"));
                }
                return Ok(new Response().Success().SetData(_mapper.Map<BookDTO>(book)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }

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
                if (book is null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Truyện không tồn tại trong hệ thống"));
                }

                var user = HttpContext.Items["UserDTO"] as UserDTO;
                if (!_bookService.Modifiable(book, user!))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response().Forbidden());
                }

                var result = await _bookService.DeleteBookAsync(book);
                if (result)
                {
                    return Ok(new Response().Success().SetMessage("Thành công!"));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }

        }

        [HttpPut]
        [Route("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDTO dto)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book is null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Truyện không tồn tại trong hệ thống"));
                }

                var user = HttpContext.Items["UserDTO"] as UserDTO;
                if (!_bookService.Modifiable(book, user!))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response().Forbidden());
                }

                var result = await _bookService.UpdateBookAsync(book, dto);
                if (result)
                {
                    return Ok(new Response().Success().SetMessage("Cập nhật thông tin truyện thành công!"));
                }
                return BadRequest(new Response().BadRequest().SetMessage("Cập nhật thông tin truyện không thành công!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
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

                return Ok(new PaginatedResponse
                {
                    PageNumber = filter.Page,
                    PageSize = filter.PageSize,
                    TotalItems = count,
                    TotalPages = (int)Math.Ceiling(count / (double)filter.PageSize),
                }.SetData(_mapper.Map<IEnumerable<BookDTO>>(paginatedBooks)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }

        [HttpPost]
        [Route("change-poster/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePoster(int id, [FromForm] ImageDTO dto)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Truyện không tồn tại trong hệ thống"));
                }

                var user = HttpContext.Items["UserDTO"] as UserDTO;
                if (!_bookService.Modifiable(book, user!))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response().Forbidden());
                }

                var url = await _imageStorageService.UploadImageAsync(dto.Image);
                var result = await _bookService.ChangePosterAsync(book, url);
                if (result)
                {
                    return Ok(new Response().Success().SetMessage("Cập nhật ảnh bìa của truyện thành công").SetData(new { Url = url }));
                }
                return BadRequest(new Response().BadRequest().SetMessage("Có chút trục trặc trong khi chúng tôi đang cố gắng thay bức hình tuyệt vời này!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }

        [HttpGet]
        [Route("{id}/chapter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Book>>> GetChaptersByBookId(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Truyện không tồn tại hoặc đã bị xóa trong hệ thống"));
                }
                var chapters = await _chapterService.GetChaptersByBookIdAsync(id);

                return Ok(new Response().Success().SetData(new
                {
                    chapters = _mapper.Map<IEnumerable<ChapterInfoDTO>>(chapters),
                    book = _mapper.Map<BookDTO>(book)
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }

        [HttpGet]
        [Route("{id}/comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByBookId(int id, [FromQuery] CommentFilter filter)
        {
            try
            {
                var comments = await _commentService.GetAllCommentsByBookIdAsync(id, filter);
                var count = comments!.Count();
                var paginatedComment = comments!.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize);

                return Ok(new PaginatedResponse
                {
                    PageNumber = filter.Page,
                    PageSize = filter.PageSize,
                    TotalItems = count,
                    TotalPages = (int)Math.Ceiling(count / (double)filter.PageSize),
                }.SetData(_mapper.Map<IEnumerable<CommentDTO>>(paginatedComment)));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }

        [HttpGet]
        [Route("{id}/review")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Comment>>> GetReviewsByBookId(int id, [FromQuery] ReviewFilter filter)
        {
            try
            {
                var reviews = await _reviewService.GetAllReviewsByBookIdAsync(id, filter);
                var count = reviews!.Count();
                var paginatedReview = reviews!.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize);

                return Ok(new PaginatedResponse
                {
                    PageNumber = filter.Page,
                    PageSize = filter.PageSize,
                    TotalItems = count,
                    TotalPages = (int)Math.Ceiling(count / (double)filter.PageSize),
                }.SetData(_mapper.Map<IEnumerable<ReviewDTO>>(paginatedReview)));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }
    }
}
