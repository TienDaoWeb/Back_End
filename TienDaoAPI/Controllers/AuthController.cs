using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TienDaoAPI.DTOs;
using TienDaoAPI.IRepositories;
using TienDaoAPI.Models;

namespace TienDaoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtRepository _jwtRepository;

        public AuthController(UserManager<User> userManager, IJwtRepository jwtRepository)
        {
            _userManager = userManager;
            _jwtRepository = jwtRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var user = new User
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username
            };
            var identityResult = await _userManager.CreateAsync(user, registerRequestDTO.Password);
            if (identityResult.Succeeded)
            {
                // Add roles to this User
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(user, registerRequestDTO.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! Please login");
                    }
                }


            }
            return BadRequest("Something wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Username);
            if (user != null)
            {
                var checkPassword = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                if (checkPassword)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = _jwtRepository.CreateJWTToken(user, roles.ToList());
                        return Ok(new LoginResponseDTO() { token = jwtToken });
                    }
                }
            }
            return BadRequest("Username or password incorrect");
        }

    }
}
