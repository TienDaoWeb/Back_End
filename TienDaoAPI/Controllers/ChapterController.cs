using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TienDaoAPI.DTOs;
using TienDaoAPI.Enums;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ChapterController : ControllerBase
    {
        private readonly IChapterService _chapterService;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public ChapterController(IChapterService chapterService, IBookService bookService, IMapper mapper)
        {
            _chapterService = chapterService;
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = RoleEnum.CONVERTER)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateChapterDTO dto)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(dto.BookId);
                if (book == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Truyện không tồn tại trong hệ thống"));
                }

                var user = HttpContext.Items["UserDTO"] as UserDTO;
                if (!_bookService.Modifiable(book, user!))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response().Forbidden());
                }

                dto.OwnerId = user!.Id;
                dto.Index = ++book.LastestIndex;
                var chapter = await _chapterService.CreateChapterAsync(dto);
                if (chapter == null)
                {
                    return BadRequest(new Response().BadRequest().SetMessage("Không thể thêm chương mới. Vui lòng kiểm tra lại thông tin."));
                }
                return Ok(new Response().Success().SetMessage("Thêm chương thành công!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }

        [HttpDelete]
        [Authorize(Roles = RoleEnum.CONVERTER)]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var chapter = await _chapterService.GetChapterByIdAsync(id);
                if (chapter == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Chương không tồn tại trong hệ thống"));
                }

                var user = HttpContext.Items["UserDTO"] as UserDTO;
                if (!_chapterService.Modifiable(chapter, user!))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response().Forbidden());
                }

                var result = await _chapterService.DeleteChapterAsync(chapter);
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

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetChapter(int id)
        {
            try
            {
                var chapter = await _chapterService.GetChapterByIdAsync(id);
                if (chapter == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Truyện không tồn tại hoặc đã bị xóa trong hệ thống"));
                }
                return Ok(new Response().Success().SetData(_mapper.Map<ChapterDetailDTO>(chapter)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }
    }
}
