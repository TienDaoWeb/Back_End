using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TienDaoAPI.DTOs.Requests;
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

        public StoryController(IFirebaseStorageService firebaseStorageService, IStoryService storyService , IChapterService chapterService)
        {
            _firebaseStorageService = firebaseStorageService;
            _storyService = storyService;
            _chapterService = chapterService;
        }

        //Create story save DB
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromForm] StoryRequestDTO storyRequest)
        {
            try
            {
                if (storyRequest.UrlImage == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "Can not find file image to create story."
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
        //Get story
        [HttpGet]
        [Route("getStory/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStory(int id)
        {
            try
            {
                var story = await _storyService.GetStoryByIdAsync(id);

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = story
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
                        Message = "Story does not exists",

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
        [Route("StoryUpdate/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StoryUpdate(int id, [FromForm] StoryRequestDTO newStory)
        {
            try
            {
                var story = await _storyService.GetStoryByIdAsync(id);
                if (story == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Not found the Story by ID",
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
        //Get list story arranged in ascending days
        //[HttpGet]
        //[Route("liststory/{sort=}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> liststory(string sort)
        //{
        //    try
        //    {

        //        var story = _data.Stories.ToList();
        //        if (sort == "asc")
        //        {
        //            // Sắp xếp sản phẩm theo ngày
        //            story.Sort((a, b) => a.CreateDate.CompareTo(b.CreateDate));
        //        }
        //        else if (sort == "dec")
        //        {
        //            // Sắp xếp sản phẩm theo ngày
        //            story.Sort((a, b) => b.CreateDate.CompareTo(a.CreateDate));
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
        //            {
        //                StatusCode = HttpStatusCode.BadGateway,
        //                Message = "Can not get list story",
        //            });

        //        }
        //        // Trả về danh sách sản phẩm đã sắp xếp
        //        return  StatusCode(StatusCodes.Status200OK, new CustomResponse
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Message = "Get list story successfully",
        //            Result = story
        //        });
        //    }
        //    catch(Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
        //        {
        //            StatusCode = HttpStatusCode.InternalServerError,
        //            IsSuccess = false,
        //            Message = "Internal Server Error: " + ex.Message
        //        });
        //    }
        //}
        [HttpPost]
        [Route("stories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Story>>> GetAllStories(string keyword, string orderBy, int genre, int page, int limit)
        {
            try
            {
                var stories = await _storyService.GetAllStoriesAsync(item => item.Title.Contains(keyword));

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Result ",
                    Result = stories
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
