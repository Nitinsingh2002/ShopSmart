using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using ShoapMart.Api.DTOs;
using ShoapMart.Api.interfaces;
using ShopMart.Api.Entities;
using ShopMart.Api.Enums;
using ShopMart.Api.Interfaces;

namespace ShoapMart.Api.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ITokenRepository _ITokenrepo;


        public AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
         ITokenRepository _ITokenrepo)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            this._ITokenrepo = _ITokenrepo;
        }

        public async Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string Password, string role)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Role {role} does not exist" });
            }
            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            return result;
        }

        public async Task<bool> UserExitsAsync(string Email)
        {
            var result = await _userManager.FindByEmailAsync(Email);
            return result != null;
        }


        public async Task<string> LoginUserAsync(LoginRequestDTO loginRequestDto)
        {
            try
            {
                ApplicationUser? user = null;

                if (loginRequestDto.LoginMethod == LoginType.Email)
                {
                    user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
                    if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
                    {
                        throw new UnauthorizedAccessException("Invalid Email or password");
                    }
                }
                else if (loginRequestDto.LoginMethod == LoginType.Phone)
                {
                    user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == loginRequestDto.PhoneNumber);
                    if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
                    {
                        throw new UnauthorizedAccessException("Invalid phone number or password");
                    }
                }
                else if (loginRequestDto.LoginMethod == LoginType.PhoneOTP)
                {
                    // OTP login logic (implement later)
                    user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == loginRequestDto.PhoneNumber);
                    if (user == null)
                    {
                        throw new UnauthorizedAccessException("invalid Phone Number");
                    }
                }

                //calling method which generate token
                var roles = await _userManager.GetRolesAsync(user);
                if (roles == null || roles.Count == 0)
                {
                    throw new InvalidOperationException("User has no roles assigned");
                }

                var token = _ITokenrepo.createToken(user, roles.First());

                return token;
            }
            catch (Exception ex)
            {
                throw;

            }
        }
    }
}