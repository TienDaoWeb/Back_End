using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.Collections;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.WebSockets;
using TienDaoAPI.Data;
using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.Models;
using TienDaoAPI.Repositories;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Response;






namespace TienDaoAPI.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly IStoryRepository _data;
        private readonly IFirebaseStorageService _filebaseStorage;

        public StoryController(IStoryRepository data , IFirebaseStorageService filebaseStorage)
        {
            _data = data;
            _filebaseStorage = filebaseStorage;
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
                if (storyRequest.UrlImage != null)
                {
                    string FileName = storyRequest.Image;
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;
                    //var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/images/", uniqueFileName);
                    //var fs =  new FileStream(imagePath, FileMode.Open);
                    //await storyRequest.UrlImage.CopyToAsync(new FileStream(imagePath, FileMode.Create));
                    _filebaseStorage.UploadFile(uniqueFileName, storyRequest.UrlImage);
                    var newStory = new Story
                    {
                        Title = storyRequest.Title,
                        Author = storyRequest.Author,
                        Description = storyRequest.Description,
                        Status = storyRequest.Status,
                        Image = uniqueFileName,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };

                    _data.CreateAsync(newStory);
          


                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Create story successfully",
                        Result = newStory
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "Can not find file image to create story."
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
        //Get story
        [HttpGet]
        [Route("getStory/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getStory(int Id)
        {
            try
            {
                var story = await _data.GetById(Id);
                if(story == null){
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message="Can not find Story to DB"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Result = story
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
        // Delete story 
        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromBody] int Id)
        {
            try
            {
                var Story = await _data.Find(item => item.Id == Id);
                if (Story != null)
                {
                    if (Story.Image != null)
                    {
                        string UploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets/images/");

                        string fileLoad = Path.Combine(UploadDir, Story.Image);
                        if (System.IO.File.Exists(fileLoad))
                        {
                            System.IO.File.Delete(fileLoad);
                        }
                    }
                    _data.RemoveAsync(Story);
                    _data.SaveAsync();
                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Delete story successfully",

                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Not Found",

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
        //Update Story
        [HttpPut]
        [Route("StoryUpdate/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StoryUpdate(int id, [FromBody] Story newStory)
        {
            try
            {
                var Story = await _data.GetById(id);
                if (Story == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Not found the Story by ID",
                    });
                }
                else
                {
                    Story.Status = newStory.Status;
                    Story.Author = newStory.Author;
                    Story.Description = newStory.Description;
                    Story.Status = newStory.Status;
                    Story.Image = newStory.Image;
                    _data.UpdateAsync(Story);
                    _data.SaveAsync();
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
        [Route("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Story>>> Search([FromBody] string name)
        {
            try
            {
                var results = await _data.Find(item => item.Title.Contains(name));
               
                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message ="Result ",
                    Result = results
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
