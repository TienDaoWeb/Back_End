﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.Models;
using TienDaoAPI.Response;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<User> _userManager;

        public AuthController(IJwtService jwtService, IUserService userService, IEmailSender emailSender, UserManager<User> userManager)
        {
            _jwtService = jwtService;
            _userService = userService;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            try
            {
                var user = new User
                {
                    Email = registerRequestDTO.Email,
                    UserName = registerRequestDTO.Email
                };

                var identityResult = await _userManager.CreateAsync(user, registerRequestDTO.Password);

                if (identityResult.Succeeded)
                {
                    // Add roles to this User
                    if (registerRequestDTO.Role != null && registerRequestDTO.Role.Any())
                    {
                        identityResult = await _userManager.AddToRoleAsync(user, registerRequestDTO.Role);
                        if (identityResult.Succeeded)
                        {
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                            // https: //localhost:8080/ConfirmEmail/userId=???&code=???
                            string callbackUrl = Request.Scheme + "://" + Request.Host +
                                Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = code });

                            await _emailSender.SendEmailAsync(user.Email, "Xác nhận địa chỉ email",
                            $"Hãy xác nhận địa chỉ email bằng cách <a href='{callbackUrl}'>Bấm vào đây</a>.");

                            return StatusCode(StatusCodes.Status200OK, new CustomResponse
                            {
                                StatusCode = HttpStatusCode.OK,
                                Message = "User was registered! Please login"
                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Some thing wrong!"
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

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user = await _userService.FindByEmailAsync(loginRequestDTO.Email);
                if (user != null)
                {
                    var checkPassword = _userService.CheckPassword(user, loginRequestDTO.Password);
                    if (checkPassword)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles != null)
                        {
                            var jwtToken = _jwtService.CreateJWTToken(user, roles.ToList());
                            return StatusCode(StatusCodes.Status200OK, new CustomResponse
                            {
                                StatusCode = HttpStatusCode.OK,
                                Message = "Login successfully",
                                Result = new { Token = jwtToken }
                            });
                        }
                    }

                    return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Password is incorrect"
                    });
                }
                return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    Message = "User does not exsit"
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
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(int userId, string code)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "User does not exsit"
                    });
                }
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var identityResult = await _userManager.ConfirmEmailAsync(user, code);
                if (identityResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Confirm email successfully"
                    });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Confirm email failed"
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
