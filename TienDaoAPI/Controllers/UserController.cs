using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TienDaoAPI.Enums;
using TienDaoAPI.Models;
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

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
        [Authorize(Roles = RoleEnum.ADMIN)]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new Response().NotFound().SetMessage("Người dùng không tồn tại trong hệ thống"));
                }
                return Ok(new Response().Success().SetData(user));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new Response().InternalServerError());
            }
        }
    }
}
