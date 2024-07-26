﻿using AutoMapper;
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
using TienDaoAPI.Services.IServices;
using TienDaoAPI.Utils;

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

        public AuthController(EmailProvider emailProvider, UserManager<User> userManager, SessionProvider sessionProvider,
        IMapper mapper, JwtHandler jwtHandler, IAccountService accountService)
        {
            _emailProvider = emailProvider;
            _userManager = userManager;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _accountService = accountService;
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
                    AccountErrorEnum.AllOk => StatusCode(StatusCodes.Status200OK, new Response
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Chúc mừng! Bạn vừa tạo ra một hồn ma mới trong hệ thống!",

                    }),
                    AccountErrorEnum.Existed => StatusCode(StatusCodes.Status400BadRequest, new Response
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Email đã tồn tại trong hệ thống của chúng tôi"
                    }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, new Response
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        IsSuccess = false,
                        Message = "Internal Server Error: Không xác định lỗi."
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
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
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);

                if (user != null)
                {
                    var checkPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
                    if (checkPassword)
                    {
                        if (!user.EmailConfirmed)
                        {
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var templatePath = "./Templates/otp_register_mail.html";
                            await _emailProvider.SendEmailWithTemplateAsync(user.Email!, "Email Verification", templatePath, new { code });

                            return StatusCode(StatusCodes.Status400BadRequest, new Response
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
                        return StatusCode(StatusCodes.Status200OK, new Response
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Login successfully",
                            Data = new { AccessToken = jwtToken, RefreshToken = refreshToken, Profile = _mapper.Map<UserDTO>(user) }
                        });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new Response
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            IsSuccess = false,
                            Message = "Password is incorrect"
                        });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "User does not exsit"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
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
        public async Task<IActionResult> ConfirmEmail([FromBody] OTPEmailDTO dto)
        {
            try
            {
                var result = await _accountService.VerifyEmailAsync(dto.Email, dto.OTP);

                return result switch
                {
                    AccountErrorEnum.AllOk => StatusCode(StatusCodes.Status200OK, new Response
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Xác thực email thành công!",

                    }),
                    AccountErrorEnum.InvalidOTP => StatusCode(StatusCodes.Status200OK, new Response
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "OTP không hợp lệ hoặc đã hết hạn!",

                    }),
                    AccountErrorEnum.NotExists => StatusCode(StatusCodes.Status400BadRequest, new Response
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "Tài khoản của bạn không tồn tại trong hệ thống của chúng tôi"
                    }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, new Response
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        IsSuccess = false,
                        Message = "Internal Server Error: Không xác định lỗi."
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
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
                    AccountErrorEnum.AllOk => StatusCode(StatusCodes.Status200OK, new Response
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = $"Vui lòng sử dụng OTP đã được gửi tới địa chỉ {dto.Email} để khôi phục mật khẩu!"
                    }),
                    AccountErrorEnum.NotExists => StatusCode(StatusCodes.Status400BadRequest, new Response
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Tài khoản của bạn không tồn tại trong hệ thống của chúng tôi"
                    }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, new Response
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        IsSuccess = false,
                        Message = "Internal Server Error: Không xác định lỗi."
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
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
        public async Task<IActionResult> ResetPassword([FromBody] OTPEmailDTO dto)
        {
            try
            {
                var result = await _accountService.ResetPasswordAsync(dto.Email, dto.OTP);

                return result switch
                {
                    AccountErrorEnum.AllOk => StatusCode(StatusCodes.Status200OK, new Response
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = $"Vui lòng sử dụng mật khẩu đã được gửi tới địa chỉ {dto.Email} để đăng nhập tài khoản!",

                    }),
                    AccountErrorEnum.InvalidOTP => StatusCode(StatusCodes.Status200OK, new Response
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "OTP không hợp lệ hoặc đã hết hạn!",

                    }),
                    AccountErrorEnum.NotExists => StatusCode(StatusCodes.Status400BadRequest, new Response
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "Tài khoản của bạn không tồn tại trong hệ thống của chúng tôi"
                    }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, new Response
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        IsSuccess = false,
                        Message = "Internal Server Error: Không xác định lỗi."
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
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
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO dto)
        {
            try
            {
                if (!_sessionProvider.VerifyRefreshToken(dto.RefreshToken))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Refresh token không chính xác hoặc hết hạn!"
                    });
                }
                var userId = new JwtSecurityTokenHandler().ReadJwtToken(dto.RefreshToken).Payload["sub"].ToString();
                var user = await _userManager.FindByIdAsync(userId!);
                var jwtToken = _jwtHandler.CreateJWTToken(user!);
                var newRefreshToken = _jwtHandler.CreateRefreshToken(user!);


                var accessJti = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken).Payload["jti"].ToString();
                var refreshJti = new JwtSecurityTokenHandler().ReadJwtToken(newRefreshToken).Payload["jti"].ToString();
                _sessionProvider.SaveSession(userId!, _mapper.Map<UserDTO>(user), accessJti!, refreshJti!);

                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new
                    {
                        AccessToken = jwtToken,
                        RefreshToken = newRefreshToken
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
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
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            try
            {
                var userId = (HttpContext.Items["UserDTO"] as UserDTO)!.Id;
                var result = await _accountService.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);

                return result switch
                {
                    AccountErrorEnum.AllOk => StatusCode(StatusCodes.Status200OK, new Response
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Thay đổi mật khẩu thành công",
                    }),
                    AccountErrorEnum.NotExists => StatusCode(StatusCodes.Status404NotFound, new Response
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Message = "Tài khoản của bạn không tồn tại trong hệ thống của chúng tôi"
                    }),
                    AccountErrorEnum.WeakPassword => StatusCode(StatusCodes.Status400BadRequest, new Response
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Mật khẩu phải dài tối thiểu 8 ký tực và tối đa 30 ký tự, bao gồm ít nhất một chữ thường, một chữ hoa, một chữ số và một ký tự đặc biệt!"
                    }),
                    AccountErrorEnum.IncorrectPassword => StatusCode(StatusCodes.Status400BadRequest, new Response
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Message = "Mật khẩu hiện tại chưa chính xác!"
                    }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, new Response
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        IsSuccess = false,
                        Message = "Internal Server Error: ex.Message"
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = "Internal Server Error: " + ex.Message
                });
            }
        }
    }
}
