using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TienDaoAPI.DTOs.Bookmarks;
using TienDaoAPI.DTOs.Users;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IBookmarkService _bookmarkService;
        private readonly IMapper _mapper;
        public BookmarkController(IBookService bookService, IBookmarkService bookmarkService, IMapper mapper)
        {
            _bookService = bookService;
            _bookmarkService = bookmarkService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateBookmarkDTO dto)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(dto.BookId);
                if (book == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Truyện không tồn tại hoặc đã bị xóa trong hệ thống"));
                }

                var user = HttpContext.Items["UserDTO"] as UserDTO;

                dto.UserId = user!.Id;
                var result = await _bookmarkService.CreateBookmarkAsync(dto);
                if (!result)
                {
                    return BadRequest(new Response().BadRequest().SetMessage("Không thể đánh dấu truyện. Vui lòng kiểm tra lại thông tin."));
                }
                return Ok(new Response().Success().SetMessage("Thêm dấu trang thành công!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookmarks([FromQuery] PaginationFilter filter)
        {
            try
            {
                var user = HttpContext.Items["UserDTO"] as UserDTO;

                var readings = await _bookmarkService.GetBookmarksByUserIdAsync(user!.Id);
                var count = readings!.Count();
                var paginatedBooks = readings!.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize);

                return Ok(new PaginatedResponse
                {
                    PageNumber = filter.Page,
                    PageSize = filter.PageSize,
                    TotalItems = count,
                    TotalPages = (int)Math.Ceiling(count / (double)filter.PageSize),
                }.SetData(_mapper.Map<IEnumerable<BookmarkDTO>>(readings)));
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Book>>> DeleteReading(int id)
        {
            try
            {
                var bookmark = await _bookmarkService.GetBookmarkByIdAsync(id);
                if (bookmark == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Dấu trang không tồn tại trong hệ thống"));
                }

                var user = HttpContext.Items["UserDTO"] as UserDTO;
                if (!_bookmarkService.Modifiable(bookmark, user!))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response().Forbidden());
                }

                var result = await _bookmarkService.DeleteBookmarkAsync(id);
                if (!result)
                {
                    BadRequest(new Response().BadRequest().SetMessage("Có lỗi xảy ra khi xóa dấu trang!"));
                }

                return Ok(new Response().Success().SetMessage("Xóa dấu trang thành công!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }
    }
}
