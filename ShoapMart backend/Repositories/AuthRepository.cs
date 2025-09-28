using Microsoft.AspNetCore.Identity;
using ShoapMart.Api.interfaces;
using ShopMart.Api.Entities;

namespace ShoapMart.Api.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
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
    }
}