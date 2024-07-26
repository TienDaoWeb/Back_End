using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TienDaoAPI.DTOs;
using TienDaoAPI.Models;
using TienDaoAPI.Utils;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;

        public GenreController(IGenreService genreService, IMapper mapper)
        {
            _genreService = genreService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var genres = await _genreService.GetAllGenresAsync();
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = genres
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateGenreDTO genre)
        {
            try
            {
                var newGenre = _mapper.Map<Genre>(genre);
                var result = await _genreService.CreateGenreAsync(newGenre);
                if (result)
                {
                    return StatusCode(StatusCodes.Status201Created, new Response
                    {
                        StatusCode = HttpStatusCode.Created,
                        Message = "Thêm thể loại thành công!",
                    });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Không thể tạo thể loại. Vui lòng thử lại sau hoặc liên hệ với quản trị viên để biết thêm chi tiết!"
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
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _genreService.DeleteGenreAsync(id);
                if (result)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Xóa thể loại thành công!",
                    });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Thể loại không tồn tại hoặc đã bị xóa!"
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
