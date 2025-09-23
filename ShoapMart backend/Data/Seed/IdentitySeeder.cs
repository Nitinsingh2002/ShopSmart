using Microsoft.AspNetCore.Identity;
using ShopMart.Api.Entities;

namespace ShoapMart.Api.Data.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //  Seed Roles
            string[] roles = { "Admin", "Vendor", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //  Seed default Admin user
            var adminEmail = "admin@shopmart.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await userManager.CreateAsync(adminUser, "Admin@123"); 
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }

}