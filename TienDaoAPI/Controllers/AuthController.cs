﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.DTOs.Response;
using TienDaoAPI.DTOs.Responses;
using TienDaoAPI.Models;
using TienDaoAPI.Response;
using TienDaoAPI.Services.IServices;

namespace TienDaoAPI.Controllers
{
    [ApiController]
    //[ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<User> _userManager;

        public AuthController(IJwtService jwtService, IUserService userService,
            IEmailSender emailSender, UserManager<User> userManager, IRefreshTokenService refreshTokenService)
        {
            _jwtService = jwtService;
            _userService = userService;
            _refreshTokenService = refreshTokenService;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequestDTO)
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
                                Message = "User was registered",

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
                    Message = "Internal Server Error: " + ex.Message,

                });
            }
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequestDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);
                if (user != null)
                {
                    var checkPassword = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                    if (checkPassword)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles != null)
                        {
                            var jwtToken = _jwtService.CreateJWTToken(user, roles.ToList());
                            RefreshToken refreshToken = await _refreshTokenService.createRefreshTokenAsync(user);
                            return StatusCode(StatusCodes.Status200OK, new CustomResponse
                            {
                                StatusCode = HttpStatusCode.OK,
                                Message = "Login successfully",
                                Result = new LoginResponse { AccessToken = jwtToken, RefreshToken = refreshToken.Token }
                            }); ;
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("confirm-email")]
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("forget-password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "User does not exsit"
                    });
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                // https: //localhost:8080/ConfirmEmail/email=???&encodedToken=???
                string callbackUrl = Request.Scheme + "://" + Request.Host +
                    Url.Action("ResetPassword", "Auth", new { email = user.Email, token = encodedToken });

                await _emailSender.SendEmailAsync(user.Email, "Đặt lại mật khẩu",
                $"Đặt lại mật khẩu bằng cách <a href='{callbackUrl}'>Bấm vào đây</a>.");

                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Send email to reset password successfully"
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequestDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordRequestDTO.Email);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "User does not exsit"
                    });
                }
                var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordRequestDTO.Token));
                var identityResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordRequestDTO.NewPassword);
                if (identityResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Reset password successfully"
                    });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Reset password failed"
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
        [Route("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                var refreshToken = await _refreshTokenService.getRefreshTokenByTokenAsync(refreshTokenRequest.RefreshToken);
                if (refreshToken == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Refresh token is incorrect"
                    });
                }
                var isTokenExpried = await _refreshTokenService.CheckTokenExpiredAsync(refreshToken);
                if (isTokenExpried)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Refresh token is expired"
                    });
                }
                var user = await _userService.GetUserByIdAsync(refreshToken.UserId);
                var roles = await _userManager.GetRolesAsync(user);
                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Reset password successfully",
                    Result = new RefreshTokenResponse
                    {
                        AccessToken = _jwtService.CreateJWTToken(user, roles.ToList()),
                        RefreshToken = refreshTokenRequest.RefreshToken
                    }
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
        [Authorize]
        [Route("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            try
            {
                string? token = await HttpContext.GetTokenAsync("access_token");
                if (token == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        IsSuccess = false,
                        Message = "Please login!"
                    });
                }
                var email = _jwtService.ExtractEmailFromToken(token);
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "User does not exist"
                    });
                }
                var result = await _userManager.ChangePasswordAsync(user, changePasswordRequest.CurrentPassword,
                    changePasswordRequest.NewPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Can't change password");
                }
                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Reset password successfully"
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
