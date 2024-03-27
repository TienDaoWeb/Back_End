using AutoMapper;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.DTOs.Responses;
using TienDaoAPI.Models;
using TienDaoAPI.Response;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IStoryService _storyService;
        private readonly IChapterService _chapterService;
        private readonly IMapper _mapper;
        private readonly IGenreService _genreService;
        private readonly IUserService _userService;

        public StoryController(IFirebaseStorageService firebaseStorageService, IStoryService storyService , IChapterService chapterService)
        public StoryController(IFirebaseStorageService firebaseStorageService, IStoryService storyService,
            IMapper mapper, IGenreService genreService, IUserService userService)
        {
            _firebaseStorageService = firebaseStorageService;
            _storyService = storyService;
            _chapterService = chapterService;
            _mapper = mapper;
            _genreService = genreService;
            _userService = userService;
        }

        //Create story save DB
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromForm] StoryRequest storyRequest)
        {
            try
            {
                if (storyRequest.UrlImage == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Can not find file image to create story."
                    });
                }
                var genre = await _genreService.GetGenreByIdAsync(storyRequest.GenreId);
                if (genre == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Genre does not exsits"
                    });
                }
                var user = await _userService.GetUserByIdAsync(storyRequest.UserId);
                if (genre == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "User does not exsits"
                    });
                }
                var newStory = await _storyService.CreateStoryAsync(storyRequest);
                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Create story successfully",
                    Result = newStory
                });
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
        [Route("/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStory(int id)
        {
            try
            {
                var story = await _storyService.GetStoryByIdAsync(id);
                if (story == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "Story does not exists"
                    });
                }

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<StoryResponse>(story)
                });
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
        // Delete story 
        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var story = await _storyService.GetStoryByIdAsync(id);
                if (story == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Story does not exist",

                    });
                }
                if (story.Image != null)
                {
                    var storage = StorageClient.Create();
                    // Xóa file ảnh
                    await storage.DeleteObjectAsync("tiendaoapi.appspot.com", $"images/{story.Image}");
                }
                //Xóa tất cả các chapter của story
                var listChapter = await _chapterService.GetAllChapteofStoryrAsync(chapter => chapter.StoryId == story.Id);
                foreach (var chapter in listChapter)
                {
                    await _chapterService.DeleteChapterAsync(chapter);
                }

                // Xóa Story 
                await _storyService.DeleteStoryAsync(story);

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Delete story successfully",
                });
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
        //Update Story
        [HttpPut]
        [Route("update/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StoryUpdate(int id, [FromForm] StoryRequest newStory)
        {
            try
            {
                var story = await _storyService.GetStoryByIdAsync(id);
                if (story == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Can't find story",
                    });
                }
                else
                {
                    story.Status = newStory.Status;
                    story.Author = newStory.Author;
                    story.Description = newStory.Description;
                    story.Status = newStory.Status;
                    if (newStory.UrlImage != null)
                    {
                        // Xóa file ảnh
                        var storage = StorageClient.Create();
                        await storage.DeleteObjectAsync("tiendaoapi.appspot.com", $"images/{story.Image}");
                        //Tạo file ảnh mới
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + newStory.Image;
                        await _firebaseStorageService.UploadFile(uniqueFileName, newStory.UrlImage);
                        story.Image = uniqueFileName;
                    }

                    await _storyService.UpdateStoryAsync(story);

                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Update story successfully",
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
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Story>>> GetAllStories([FromQuery] StoryQueryObject storyQueryObject)
        {
            try
            {
                var stories = await _storyService.GetAllStoriesAsync(storyQueryObject);

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Result ",
                    Result = _mapper.Map<IEnumerable<StoryResponse>>(stories)
                });
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
