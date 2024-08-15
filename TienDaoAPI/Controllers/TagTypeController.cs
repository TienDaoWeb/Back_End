using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TienDaoAPI.DTOs.TagTypes;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TagTypeController : ControllerBase
    {
        private readonly ITagTypeService _tagTypeService;
        private readonly IMapper _mapper;
        public TagTypeController(ITagTypeService tagTypeService, IMapper mapper)
        {
            _tagTypeService = tagTypeService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tagTypes = await _tagTypeService.GetTagTypesAsync();
                return Ok(new Response().Success().SetData(tagTypes));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateTagTypeDTO dto)
        {
            try
            {
                var tagType = _mapper.Map<TagType>(dto);
                var result = await _tagTypeService.CreateTagTypeAsync(tagType);
                if (result)
                {
                    return Ok(new Response().Success().SetMessage("Thành công!"));
                }
                return BadRequest(new Response().BadRequest().SetMessage("Không thể tạo loại tag. Vui lòng thử lại sau hoặc liên hệ với quản trị viên để biết thêm chi tiết!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
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
                var result = await _tagTypeService.DeleteTagTypeAsync(id);
                if (result)
                {
                    return Ok(new Response().Success().SetMessage("Thành công!"));
                }
                return BadRequest(new Response().BadRequest().SetMessage("Loại tag không tồn tại hoặc đã bị xóa!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }
    }
}
