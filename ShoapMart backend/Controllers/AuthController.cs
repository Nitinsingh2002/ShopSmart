using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoapMart.Api.DTOs;
using ShoapMart.Api.interfaces;
using ShopMart.Api.Entities;

namespace ShopMart.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _IAuthrepo;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository _IAuthrepo, IMapper _mapper)
        {
            this._IAuthrepo = _IAuthrepo;
            this._mapper = _mapper;
        }


        //API for registering user
        [HttpPost("/auth/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
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
                // collect error messages from IdentityResult
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { Success = false, Message = "Registration failed", Errors = errors });
            }

            return Ok("User registered successfully");
        }



        //API for login user
        // [HttpPost("/auth/login")]
        // public IActionResult Login([FromBody] LoginRequestDto loginRequestDto)
        // {
        //     return Ok("Login endpoint");
        // }

    }
}