using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TienDaoAPI.DTOs;
using TienDaoAPI.Enums;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IBookService _bookService;
        private readonly IChapterService _chapterService;
        private readonly IMapper _mapper;
        public CommentController(ICommentService commentService, IMapper mapper, IBookService bookService, IChapterService chapterService)
        {
            _commentService = commentService;
            _mapper = mapper;
            _bookService = bookService;
            _chapterService = chapterService;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateCommentDTO dto)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(dto.BookId);
                if (book == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Truyện không tồn tại hoặc đã bị xóa trong hệ thống"));
                }

                var chapter = await _chapterService.GetChapterByIdAsync(dto.ChapterId);
                if (chapter == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Chương không tồn tại hoặc đã bị xóa trong hệ thống"));
                }
                dto.OwnerId = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;

                var result = await _commentService.CreateCommentAsync(dto);
                if (!result)
                {
                    return BadRequest(new Response().BadRequest().SetMessage("Không thể tạo bình luận mới. Vui lòng kiểm tra lại thông tin."));
                }
                return Ok(new Response().Success().SetMessage("Tạo bình luận thành công!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = RoleEnum.CONVERTER)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var OwnerId = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;
                var comment = await _commentService.GetCommentbyIdAsync(id);
                if (comment == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Bình luận không tồn tại hoặc đã bị xóa!"));
                }
                else
                {
                    if (comment.OwnerId != OwnerId)
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, new Response().Forbidden());
                    }
                    var result = await _commentService.DeleteCommentAsync(id);

                    if (result)
                    {
                        return Ok(new Response().Success().SetMessage("Thành công!"));
                    }
                    return BadRequest(new Response().BadRequest().SetMessage("Bình luận không tồn tại hoặc đã bị xóa!"));
                }
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] UpdateCommentDTO dto, int id)
        {
            try
            {
                var OwnerId = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;
                var comment = await _commentService.GetCommentbyIdAsync(id);
                if (comment is null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Không tồn tại bình luận!"));
                }
                else
                {
                    if (comment.OwnerId != OwnerId)
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, new Response().Forbidden());
                    }

                    var result = await _commentService.UpdateCommentAsync(comment, dto);
                    if (result)
                    {
                        return Ok(new Response().Success().SetMessage("Thành công!"));
                    }
                    return BadRequest(new Response().BadRequest().SetMessage("Bình luận không tồn tại hoặc đã bị xóa!"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }

        [HttpPost]
        [Route("reply")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReplyComment([FromBody] CreateReplyCommentDTO dto)
        {
            try
            {
                dto.OwnerId = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;
                var commentParent = await _commentService.GetCommentbyIdAsync(dto.CommentParentId);
                if (commentParent is null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Không tìm thấy bình luận để phản hồi!"));
                }

                var result = await _commentService.ReplyComment(dto);
                if (!result)
                {
                    return NotFound(new Response().NotFound().SetMessage("Không thể tạo phản hồi bình luận. Vui lòng kiểm tra lại thông tin."));
                }
                return Ok(new Response().Success().SetMessage("Tạo bình luận phản hồi thành công!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }
        [HttpPost]
        [Route("reaction-comment/{id}")]
        [Authorize(Roles = RoleEnum.CONVERTER)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Reactions(int id)
        {
            try
            {
                var comment = await _commentService.GetCommentbyIdAsync(id);
                var userLike = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;

                if (comment == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Không tìm thấy bình luận để phản hồi!"));
                }
                else
                {
                    var result = await _commentService.UserLikeComment(id, userLike);
                    return result switch
                    {
                        ReactionEnum.Like => Ok(new Response().Success().SetMessage("Bạn đã yêu thích bình luận này!")),
                        ReactionEnum.UnLike => Ok(new Response().Success().SetMessage("Bạn đã bỏ yêu thích comment này")),
                        ReactionEnum.Fail => BadRequest(new Response().BadRequest().SetMessage("Không thể tương tác với comment này!")),
                        _ => StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError())
                    };
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }
        [HttpGet]
        [Route("comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Comment>>> Comments([FromQuery] CommentFilter filter)
        {
            try
            {

                var comments = await _commentService.GetAllCommentAsync(filter);
                var count = comments!.Count();
                var paginatedComment = comments!.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize);

                return Ok(new PaginatedResponse
                {
                    PageNumber = filter.Page,
                    PageSize = filter.PageSize,
                    TotalItems = count,
                    TotalPages = (int)Math.Ceiling(count / (double)filter.PageSize),
                }.SetData(_mapper.Map<IEnumerable<Comment>>(paginatedComment)));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }

    }
}
