using Microsoft.AspNetCore.Mvc;
using System.Net;
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
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;
        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] GenreRequestDTO genre) {
            try
            {
                if (genre != null)
                {
                    var newgenre = new Genre
                    {
                        Name = genre.Name,
                        Description = genre.Description
                    };
                    await _genreRepository.CreateAsync(newgenre);
          
                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Create genre successfully",
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Model state is null"
                    });

                }
                

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Internal Server Error: " + ex.Message
                });
            }
        }
        [HttpDelete]
        [Route("Delete/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var genre = await _genreRepository.Find(item=>item.Id == Id);

                if (genre != null)
                {
                    await _genreRepository.RemoveAsync(genre);
                    
                    return  StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Delete genre successfully",
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
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Internal Server Error: " + ex.Message
                });
            }
        }
        [HttpPost]
        [Route("update/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] GenreRequestDTO genre,int Id)
        {
            try
            {
                var genreold = _genreRepository.GetById(Id);
                if (genre != null)
                {
                    if (genreold != null)
                    {
                        
                        return StatusCode(StatusCodes.Status200OK, new CustomResponse
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Create genre successfully",
                        });
                    }
                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Create genre successfully",
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Model state is null"
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
