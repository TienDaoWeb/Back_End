using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TienDaoAPI.Enums;
using TienDaoAPI.Models;
using TienDaoAPI.Response;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public UserController(IJwtService jwtService, IUserService userService, UserManager<User> userManager)
        {
            _jwtService = jwtService;
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = RoleEnum.ADMIN)]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                var user = HttpContext.Items["UserDTO"];

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Máy chủ đang gặp lỗi: " + ex.Message
                });
            }
        }
    }
}
