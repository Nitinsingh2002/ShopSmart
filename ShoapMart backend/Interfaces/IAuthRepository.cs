using Microsoft.AspNetCore.Identity;
using ShopMart.Api.Entities;

namespace ShoapMart.Api.interfaces
{
    public interface IAuthRepository
    {
        Task<bool> UserExitsAsync(string Email);
        Task<IdentityResult> RegisterUserAsync(ApplicationUser user,string password,string role);
    }
}
