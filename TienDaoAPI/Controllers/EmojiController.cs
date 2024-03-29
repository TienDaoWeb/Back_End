using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.DTOs.Responses;
using TienDaoAPI.Models;
using TienDaoAPI.Response;
using TienDaoAPI.Services;
using TienDaoAPI.Services.IServices;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace TienDaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmojiController : ControllerBase
    {
        private readonly IEmojiService _emojiService;
        private readonly IChapterService _chapterService;
        public EmojiController(IEmojiService emojiService , IChapterService chapterService)
        {
            _emojiService = emojiService;
            _chapterService = chapterService;
        }

        [HttpPost]
        [Route("sentiment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Sentiment([FromBody] EmojiPostRequestDTO emojiPostRequest)
        {
            try
            {
                var chapter = await _chapterService.GetChapterByIdAsync(emojiPostRequest.ChapterId);
                if(chapter != null)
                {
                    await _emojiService.UserCreateSentimentChapter(emojiPostRequest);
                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Update sentiment of chapter is successfully",
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                       
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Internal Server Error: " + ex.Message
                });
            }
        }

        [HttpGet]
        [Route("classcifySentiment/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> classcifySentiment(int id)
        {
            try
            {
                var chapter = await _chapterService.GetChapterByIdAsync(id);
                if (chapter != null)
                {
                    var classcifySentiment = await _emojiService.CountClassifySentinment(id);
                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Result= classcifySentiment
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,

                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Internal Server Error: " + ex.Message
                });
            }
        }

    }
}
