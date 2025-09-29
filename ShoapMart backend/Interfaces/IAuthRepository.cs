using Microsoft.AspNetCore.Identity;
using ShoapMart.Api.DTOs;
using ShopMart.Api.DTOs;
using ShopMart.Api.Entities;

namespace ShoapMart.Api.interfaces
{
    public interface IAuthRepository
    {
        Task<bool> UserExitsAsync(string Email);
        Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password, string role);

        Task<string> LoginUserAsync(LoginRequestDTO loginRequestDTO);

        Task<bool> SendOtpAsync(string phoneNumber);
        
        Task<string> ValidateOtpAsync(ValidateOtpRequestDTO validateOtpRequestDTO);
    }
}
