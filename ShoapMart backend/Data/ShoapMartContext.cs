using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopMart.Api.Entities;

namespace ShoapMart.Api.Data
{
    public class ShopMartContext : IdentityDbContext<ApplicationUser>
    {
        public ShopMartContext(DbContextOptions<ShopMartContext> options) : base(options) { }

    }
}