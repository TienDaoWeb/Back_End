using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TienDaoAPI.DTOs;
using TienDaoAPI.DTOs.Requests;
using TienDaoAPI.Enums;
using TienDaoAPI.Helpers;
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
        private readonly JwtHandler _jwtHandler;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly EmailProvider _emailProvider;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IRedisCacheService _redisCacheService;

        public AuthController(IJwtService jwtService, IUserService userService,
            EmailProvider emailProvider, UserManager<User> userManager, IRefreshTokenService refreshTokenService,
            IMapper mapper, JwtHandler jwtHandler, IAccountService accountService, IRedisCacheService redisCacheService)
        {
            _jwtService = jwtService;
            _userService = userService;
            _refreshTokenService = refreshTokenService;
            _emailProvider = emailProvider;
            _userManager = userManager;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _accountService = accountService;
            _redisCacheService = redisCacheService;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {

            try
            {
                var user = _mapper.Map<User>(registerDTO);
                var result = await _accountService.CreateNewAccountAsync(user, registerDTO.Password);

                return result switch
                {
                    AccountErrorEnum.AllOk => StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Chúc mừng! Bạn vừa tạo ra một hồn ma mới trong hệ thống!",

                    }),
                    AccountErrorEnum.NotExists => StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Role không tồn tại!"
                    }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        IsSuccess = false,
                        Message = "Internal Server Error: Không xác định lỗi."
                    })
                };
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
                        if (!user.EmailConfirmed)
                        {
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var templatePath = "./Templates/register.html";
                            await _emailProvider.SendEmailWithTemplateAsync(user.Email!, "Email Verification", templatePath, new { code });

                            return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                IsSuccess = false,
                                Message = "Unverified Email"
                            });
                        }
                        _redisCacheService.DeleteKeysByPattern($"refresh_token:{user.Id}:*");

                        var jwtToken = _jwtHandler.CreateJWTToken(user);
                        var refreshToken = _jwtHandler.CreateRefreshToken(user);
                        var userDto = _mapper.Map<UserDto>(user);
                        var userDtoJson = JsonConvert.SerializeObject(userDto);
                        HttpContext.Session.SetString("UserDto", userDtoJson);
                        _redisCacheService.Cache($"refresh_token:{user.Id}:{refreshToken}", userDtoJson, TimeSpan.FromDays(50));
                        return StatusCode(StatusCodes.Status200OK, new CustomResponse
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Login successfully",
                            Result = new { AccessToken = jwtToken, RefreshToken = refreshToken }
                        });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            IsSuccess = false,
                            Message = "Password is incorrect"
                        });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "User does not exsit"
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            try
            {
                var result = await _accountService.VerifyEmailAsync(email, code);

                return result switch
                {
                    AccountErrorEnum.AllOk => StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Xác thực email thành công!",

                    }),
                    AccountErrorEnum.InvalidOTP => StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "OTP không hợp lệ hoặc đã hết hạn!",

                    }),
                    AccountErrorEnum.NotExists => StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "Tài khoản không tồn tại!"
                    }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        IsSuccess = false,
                        Message = "Internal Server Error: Không xác định lỗi."
                    })
                };
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

                //await _emailSender.SendEmailAsync(user.Email, "Đặt lại mật khẩu",
                //$"Đặt lại mật khẩu bằng cách <a href='{callbackUrl}'>Bấm vào đây</a>.");

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
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            try
            {
                var userId = _jwtHandler.GetUserIdFromToken(refreshToken);
                var result = _redisCacheService.Get($"refresh_token:{userId}:{refreshToken}");

                if (result == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Refresh token không chính xác hoặc hết hạn!"
                    });
                }


                var user = await _userManager.FindByIdAsync(userId.ToString()!);
                _redisCacheService.DeleteKeysByPattern($"refresh_token:{userId}:*");
                var newRefreshToken = _jwtHandler.CreateRefreshToken(user!);
                _redisCacheService.Cache($"refresh_token:{user?.Id}:{newRefreshToken}", result, TimeSpan.FromDays(50));
                return StatusCode(StatusCodes.Status200OK, new CustomResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = new
                    {
                        AccessToken = _jwtHandler.CreateJWTToken(user!),
                        RefreshToken = newRefreshToken
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
