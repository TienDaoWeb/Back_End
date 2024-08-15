using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TienDaoAPI.DTOs;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReadingController : ControllerBase
    {
        private readonly IChapterService _chapterService;
        private readonly IReadingService _readingService;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public ReadingController(IChapterService chapterService, IReadingService readingService,
            IBookService bookService, IMapper mapper)
        {
            _chapterService = chapterService;
            _readingService = readingService;
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateReadingDTO dto)
        {
            try
            {
                var chapter = await _chapterService.GetChapterByIdAsync(dto.ChapterId);
                if (chapter == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Chương không tồn tại hoặc đã bị xóa trong hệ thống"));
                }

                var user = HttpContext.Items["UserDTO"] as UserDTO;

                dto.UserId = user!.Id;
                var result = await _readingService.CreateReadingAsync(dto, chapter);
                if (!result)
                {
                    return BadRequest(new Response().BadRequest().SetMessage("Không thể thêm chương đang đọc. Vui lòng kiểm tra lại thông tin."));
                }
                return Ok(new Response().Success().SetMessage("Thêm chương đang đọc thành công!"));
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
        public async Task<ActionResult<IEnumerable<Book>>> GetReadings([FromQuery] PaginationFilter filter)
        {
            try
            {
                var user = HttpContext.Items["UserDTO"] as UserDTO;

                var readings = await _readingService.GetReadingsByUserIdAsync(user!.Id);
                var count = readings!.Count();
                var paginatedBooks = readings!.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize);

                return Ok(new PaginatedResponse
                {
                    PageNumber = filter.Page,
                    PageSize = filter.PageSize,
                    TotalItems = count,
                    TotalPages = (int)Math.Ceiling(count / (double)filter.PageSize),
                }.SetData(_mapper.Map<IEnumerable<ReadingDTO>>(readings)));
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
                var reading = await _readingService.GetReadingByIdAsync(id);
                if (reading == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Chương đang đọc không tồn tại trong hệ thống"));
                }

                var user = HttpContext.Items["UserDTO"] as UserDTO;
                if (!_readingService.Modifiable(reading, user!))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response().Forbidden());
                }

                var result = await _readingService.DeleteReadingAsync(id);
                if (!result)
                {
                    BadRequest(new Response().BadRequest().SetMessage("Có lỗi xảy ra khi xóa chương đang đọc!"));
                }

                return Ok(new Response().Success().SetMessage("Xóa chương đang đọc thành công!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }
    }
}
