using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoapMart.Api.DTOs;
using ShoapMart.Api.interfaces;
using ShopMart.Api.Entities;
using ShopMart.Api.Interfaces;

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



        // API for login user
        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDto)
        {
            try
            {
                var token = await _IAuthrepo.LoginUserAsync(loginRequestDto);
                return Ok(new
                {
                    success = true,
                    token = token
                });

            }
            catch (UnauthorizedAccessException uaEx)
            {
                return Unauthorized(new { success = false, message = uaEx.Message });
            }
            catch (InvalidOperationException ioEx)
            {
                return BadRequest(new { success = false, message = ioEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred during login. Please try again later.",
                    detailError = ex.Message
                });
            }
        }

    }
}