using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopMart.Api.Entities;

namespace ShoapMart.Api.Data
{
    public class ShopMartContext : IdentityDbContext<ApplicationUser>
    {
        public ShopMartContext(DbContextOptions<ShopMartContext> options) : base(options) { }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<ApplicationUser>()
            .HasOne(u => u.CustomerProfile)
            .WithOne(c => c.User)
            .HasForeignKey<Customer>(c => c.UserId);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.VendorProfile)
                .WithOne(v => v.User)
                .HasForeignKey<Vendor>(v => v.UserId);

        }

    }
}