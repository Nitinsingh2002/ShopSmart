using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoapMart.Api.DTOs;
using ShoapMart.Api.interfaces;
using ShopMart.Api.Entities;
using ShopMart.Api.Interfaces;
using ShopMart.Api.DTOs;

namespace ShopMart.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _IAuthrepo;


        public AuthController(IAuthRepository _IAuthrepo)
        {
            this._IAuthrepo = _IAuthrepo;
        }


        //API for registering user
        [HttpPost("/auth/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            try
            {
                var Email = registerRequestDto.Email;
                bool checkMailExist = await _IAuthrepo.UserExitsAsync(Email);

                if (checkMailExist == true)
                {
                    return BadRequest("User already exists");
                }
                ApplicationUser user;

                if (registerRequestDto.Role == "User")
                {
                    user = new ApplicationUser
                    {
                        FirstName = registerRequestDto.FirstName,
                        LastName = registerRequestDto.LastName,
                        Email = registerRequestDto.Email,
                        UserName = registerRequestDto.Email,
                        PhoneNumber = registerRequestDto.PhoneNumber,
                        CustomerProfile = new Customer
                        {
                            DateOfBirth = registerRequestDto.DateOfBirth
                        }
                    };
                }
                else if (registerRequestDto.Role == "Vendor")
                {
                    user = new ApplicationUser
                    {
                        FirstName = registerRequestDto.FirstName,
                        LastName = registerRequestDto.LastName,
                        Email = registerRequestDto.Email,
                        UserName = registerRequestDto.Email,
                        PhoneNumber = registerRequestDto.PhoneNumber,
                        VendorProfile = new Vendor
                        {
                            ShoapName = registerRequestDto.ShopName,
                            IsVendorVerified = registerRequestDto.IsVendorVerified ?? true
                        }
                    };
                }
                else
                {
                    user = new ApplicationUser
                    {
                        FirstName = registerRequestDto.FirstName,
                        LastName = registerRequestDto.LastName,
                        Email = registerRequestDto.Email,
                        UserName = registerRequestDto.Email,
                        PhoneNumber = registerRequestDto.PhoneNumber
                    };
                }

                var result = await _IAuthrepo.RegisterUserAsync(user, registerRequestDto.Password, registerRequestDto.Role);


                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Registration failed",
                        Errors = errors
                    });
                }

                return Ok(new { Success = true, Message = "User registered successfully" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An unexpected error occurred",
                    Details = ex.Message
                });
            }
        }



        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDto)
        {
            try
            {
                var token = await _IAuthrepo.LoginUserAsync(loginRequestDto);
                return Ok(new
                {
                    Success = true,
                    Token = token
                });
            }
            catch (UnauthorizedAccessException uaEx)
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = uaEx.Message
                });
            }
            catch (InvalidOperationException ioEx)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ioEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An unexpected error occurred during login.",
                    Details = ex.Message
                });
            }
        }



        //API for send OTP[HttpPost("send-otp")]
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] string email)
        {
            try
            {
                await _IAuthrepo.SendOtpAsync(email);
                return Ok(new { Success = true, Message = "OTP sent successfully" });
            }
            catch (UnauthorizedAccessException uaEx)
            {
                return Unauthorized(new { Success = false, Message = uaEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Failed to send OTP", Details = ex.Message });
            }
        }


        //Api for validate OTP

        [HttpPost("validate-otp")]
        public async Task<IActionResult> ValidateOTP([FromBody] ValidateOtpRequestDTO validateOtpRequestDTO)
        {
            try
            {
                var token = await _IAuthrepo.ValidateOtpAsync(validateOtpRequestDTO);

                return Ok(new
                {
                    Success = true,
                    Message = "OTP validated successfully",
                    Token = token
                });
            }
            catch (UnauthorizedAccessException uaEx)
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = uaEx.Message
                });
            }
            catch (InvalidOperationException ioEx)
            {
                // User has no roles assigned
                return BadRequest(new
                {
                    Success = false,
                    Message = ioEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while validating OTP",
                    Details = ex.Message
                });
            }
        }

    }
}