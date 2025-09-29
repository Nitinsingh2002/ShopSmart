using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using ShoapMart.Api.Data;
using ShoapMart.Api.DTOs;
using ShoapMart.Api.interfaces;
using ShopMart.Api.DTOs;
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
        private readonly ShopMartContext _context;
        private readonly IOTPRepository _IOTPrepo;

        public AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
         ITokenRepository _ITokenrepo, ShopMartContext _context, IOTPRepository _IOTPrepo)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            this._ITokenrepo = _ITokenrepo;
            this._context = _context;
            this._IOTPrepo = _IOTPrepo;
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

            // Check roles
            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Count == 0)
            {
                throw new InvalidOperationException("User has no roles assigned");
            }

            // Generate token
            var token = _ITokenrepo.createToken(user, roles.First());
            return token;
        }



        public async Task<bool> SendOtpAsync(string email)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new UnauthorizedAccessException("User with the provided email does not exist.");

            var OTP = await _IOTPrepo.GenerateOTPAsync(email);

            var userOtp = new UserOTP
            {
                UserId = user.Id,
                Otp = OTP,
                CreatedAt = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            _context.UserOtps.Add(userOtp);
            await _context.SaveChangesAsync();

            var sent = await _IOTPrepo.SendOTPAsync(email, OTP, user.FirstName);

            return true;
        }


        public async Task<string> ValidateOtpAsync(ValidateOtpRequestDTO validateOtpRequestDTO)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == validateOtpRequestDTO.Email);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            // Get latest unused OTP
            var userOtp = await _context.UserOtps
                .Where(o => o.UserId == user.Id && !o.IsUsed)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (userOtp == null)
                throw new UnauthorizedAccessException("Invalid or expired OTP");

            // Check OTP
            if (userOtp.Otp == validateOtpRequestDTO.Otp && userOtp.ExpiryTime > DateTime.UtcNow)
            {
                userOtp.IsUsed = true;
                await _context.SaveChangesAsync();

                // Get roles
                var roles = await _userManager.GetRolesAsync(user);
                if (roles == null || roles.Count == 0)
                    throw new InvalidOperationException("User has no roles assigned");

                // Generate token
                var token = _ITokenrepo.createToken(user, roles.First());
                return token;
            }

            // Wrong OTP: increment attempt
            userOtp.AttemptCount++;
            await _context.SaveChangesAsync();

            throw new UnauthorizedAccessException("Invalid or expired OTP");
        }


    }
}