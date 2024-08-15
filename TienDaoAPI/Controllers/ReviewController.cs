using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TienDaoAPI.DTOs.Reviews;
using TienDaoAPI.DTOs.Users;
using TienDaoAPI.Enums;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(CreateReviewDTO dto)
        {
            try
            {
                dto.OwnerId = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;
                var result = await _reviewService.CreateReviewAsync(dto);

                if (result)
                {
                    return BadRequest(new Response().BadRequest().SetMessage("Không thể tạo bài đánh giá này"));
                }
                return Ok(new Response().Success().SetMessage("Tạo bài đánh giá thành công."));

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var review = await _reviewService.GetReviewByIdAsync(id);
                if (review == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Không thể tìm thấy bài đánh giá này"));
                }

                bool result = await _reviewService.DeleteReviewAsync(review);
                if (result == false)
                {
                    return BadRequest(new Response().BadRequest().SetMessage("Bạn không thể xóa hoặc không có quyền xóa bài viết này."));
                }
                return Ok(new Response().Success().SetMessage("Xóa bài đánh giá thành công."));
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewDTO dto)
        {
            try
            {
                var review = await _reviewService.GetReviewByIdAsync(id);
                if (review is null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Không tìm thấy bài đánh giá."));
                }

                var OwnerId = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;
                if (OwnerId != review.OwnerId)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new Response().Forbidden());
                }

                var result = await _reviewService.UpdateReviewAsync(review, dto);
                if (result)
                {
                    return BadRequest(new Response().BadRequest().SetMessage("Không thể cập nhật hay chỉnh sửa bài đánh giá."));
                }

                return Ok(new Response().Success().SetMessage("Cập nhật bài đánh giá thành công."));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }

        [HttpPost]
        [Route("{id}/like")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReactionReview(int id)
        {
            try
            {
                var review = await _reviewService.GetReviewByIdAsync(id);
                if (review == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Không tìm thấy bài đánh giá"));
                }

                var OwnerId = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;
                var result = await _reviewService.LikeOrUnlikeReview(id, OwnerId);
                return result switch
                {
                    ReactionEnum.Like => Ok(new Response().Success().SetMessage("Bạn đã yêu thích bài đánh giá này!")),
                    ReactionEnum.UnLike => Ok(new Response().Success().SetMessage("Bạn đã bỏ yêu thích bài đánh giá này")),
                    ReactionEnum.Fail => BadRequest(new Response().BadRequest().SetMessage("Không thể tương tác với đánh giá này!")),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError())
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }
    }
}
