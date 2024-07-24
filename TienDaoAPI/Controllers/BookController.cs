using AutoMapper;
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
    [Route("api/v1/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IImageStorageService _firebaseStorageService;
        private readonly IBookService _bookService;
        private readonly IChapterService _chapterService;
        private readonly IMapper _mapper;
        private readonly IGenreService _genreService;
        private readonly IUserService _userService;

        public BookController(IImageStorageService firebaseStorageService, IBookService bookService,
            IMapper mapper, IGenreService genreService, IUserService userService, IChapterService chapterService)
        {
            _firebaseStorageService = firebaseStorageService;
            _bookService = bookService;
            _chapterService = chapterService;
            _mapper = mapper;
            _genreService = genreService;
            _userService = userService;
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromForm] CreateBookDto bookRequest)
        {
            try
            {
                if (bookRequest.PosterUrl == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Can not find file image to create book."
                    });
                }
                var genre = await _genreService.GetGenreByIdAsync(bookRequest.GenreId);
                if (genre == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Genre does not exsits"
                    });
                }
                var user = await _userService.GetUserByIdAsync(bookRequest.OwnerId);
                if (genre == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "User does not exsits"
                    });
                }
                var newBook = await _bookService.CreateBookAsync(bookRequest);
                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Create book successfully",
                    Result = newBook
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
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "Book does not exists"
                    });
                }

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<BookResponse>(book)
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
        // Delete book 
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Book does not exist",

                    });
                }
                if (book.PosterUrl != null)
                {
                    //var storage = StorageClient.Create();
                    //// Xóa file ảnh
                    //await storage.DeleteObjectAsync("tiendaoapi.appspot.com", $"images/{book.PosterUrl}");
                }
                //Xóa tất cả các chapter của book
                var listChapter = await _chapterService.GetAllChapterOfBookAsync(chapter => chapter.BookId == book.Id);
                if (listChapter != null)
                {
                    var nonNullChapters = listChapter.Where(chapter => chapter != null).Cast<Chapter>();
                    await _chapterService.DeleteAllChapterAsync(nonNullChapters);
                }

                // Xóa Book 
                await _bookService.DeleteBookAsync(book);

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Delete book successfully",
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
        //Update Book
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BookUpdate(int id, [FromForm] CreateBookDto newBook)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Can't find book",
                    });
                }
                else
                {
                    book.Author = newBook.Author;
                    book.Description = newBook.Description;
                    if (newBook.PosterUrl != null)
                    {
                        // Xóa file ảnh
                        //var storage = StorageClient.Create();
                        //await storage.DeleteObjectAsync("tiendaoapi.appspot.com", $"images/{book.PosterUrl}");
                        ////Tạo file ảnh mới
                        //string uniqueFileName = Guid.NewGuid().ToString() + "_" + newBook.PosterUrl;
                        //await _firebaseStorageService.UploadImageAsync(uniqueFileName, newBook.PosterUrl);
                        //book.PosterUrl = uniqueFileName;
                    }

                    await _bookService.UpdateBookAsync(book);

                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Update book successfully",
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
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks([FromQuery] BookQueryObject bookQueryObject)
        {
            try
            {
                var stories = await _bookService.GetAllBooksAsync(bookQueryObject);

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Result ",
                    Result = _mapper.Map<IEnumerable<BookResponse>>(stories)
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
