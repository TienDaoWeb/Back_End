using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
        private readonly JwtHandler _jwtHandler;
        private readonly SessionProvider _sessionProvider;
        private readonly EmailProvider _emailProvider;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IRedisCacheService _redisCacheService;

        public AuthController(EmailProvider emailProvider, UserManager<User> userManager, SessionProvider sessionProvider,
        IMapper mapper, JwtHandler jwtHandler, IAccountService accountService, IRedisCacheService redisCacheService)
        {
            _emailProvider = emailProvider;
            _userManager = userManager;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _accountService = accountService;
            _redisCacheService = redisCacheService;
            _sessionProvider = sessionProvider;
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
                            var templatePath = "./Templates/otp_register_mail.html";
                            await _emailProvider.SendEmailWithTemplateAsync(user.Email!, "Email Verification", templatePath, new { code });

                            return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                IsSuccess = false,
                                Message = "Unverified Email"
                            });
                        }

                        var jwtToken = _jwtHandler.CreateJWTToken(user);
                        var refreshToken = _jwtHandler.CreateRefreshToken(user);

                        var userDto = _mapper.Map<UserDTO>(user);

                        var accessJti = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken).Payload["jti"].ToString();
                        var refreshJti = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken).Payload["jti"].ToString();

                        _sessionProvider.SaveSession(user.Id.ToString(), _mapper.Map<UserDTO>(user), accessJti!, refreshJti!);
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
        public async Task<IActionResult> ConfirmEmail([FromBody] string email, string otp)
        {
            try
            {
                var result = await _accountService.VerifyEmailAsync(email, otp);

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
                        Message = "Email không tồn tại!"
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
        public async Task<IActionResult> ForgetPassword([FromBody] EmailDTO dto)
        {
            try
            {
                var result = await _accountService.RequestResetPasswordAsync(dto.Email);

                return result switch
                {
                    AccountErrorEnum.AllOk => StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = $"Vui lòng sử dụng OTP đã được gửi tới địa chỉ {dto.Email} để khôi phục mật khẩu!"
                    }),
                    AccountErrorEnum.NotExists => StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Email không tồn tại!"
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            try
            {
                var result = await _accountService.ResetPasswordAsync(dto.Email, dto.OTP);

                return result switch
                {
                    AccountErrorEnum.AllOk => StatusCode(StatusCodes.Status200OK, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = $"Vui lòng sử dụng mật khẩu đã được gửi tới địa chỉ {dto.Email} để đăng nhập tài khoản!",

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
                        Message = "Email không tồn tại!"
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
        [Route("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            try
            {
                if (!_sessionProvider.VerifyRefreshToken(refreshToken))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Refresh token không chính xác hoặc hết hạn!"
                    });
                }
                var userId = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken).Payload["sub"].ToString();
                var user = await _userManager.FindByIdAsync(userId!);
                var jwtToken = _jwtHandler.CreateJWTToken(user!);
                var newRefreshToken = _jwtHandler.CreateRefreshToken(user!);


                var accessJti = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken).Payload["jti"].ToString();
                var refreshJti = new JwtSecurityTokenHandler().ReadJwtToken(newRefreshToken).Payload["jti"].ToString();
                _sessionProvider.SaveSession(userId!, _mapper.Map<UserDTO>(user), accessJti!, refreshJti!);

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
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordRequest)
        {
            try
            {
                var userJSON = HttpContext.Items["UserDto"] as UserDTO;
                //var user = JsonConvert.DeserializeObject<User>(userJSON);
                //if (token == null)
                //{
                //    return StatusCode(StatusCodes.Status401Unauthorized, new CustomResponse
                //    {
                //        StatusCode = HttpStatusCode.Unauthorized,
                //        IsSuccess = false,
                //        Message = "Please login!"
                //    });
                //}
                //var email = _jwtService.ExtractEmailFromToken(token);
                //var user = await _userManager.FindByEmailAsync(email);
                //if (user == null)
                //{
                //    return StatusCode(StatusCodes.Status404NotFound, new CustomResponse
                //    {
                //        StatusCode = HttpStatusCode.NotFound,
                //        IsSuccess = false,
                //        Message = "User does not exist"
                //    });
                //}
                //var result = await _userManager.ChangePasswordAsync(user, changePasswordRequest.CurrentPassword,
                //    changePasswordRequest.NewPassword);
                //if (!result.Succeeded)
                //{
                //    throw new Exception("Can't change password");
                //}
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
