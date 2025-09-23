using Microsoft.AspNetCore.Identity;
using ShopMart.Api.Entities;

namespace ShopMart.Api.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string  ? LastName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public Vendor? VendorProfile { get; set; }
        public Customer? CustomerProfile { get; set; }

    }
}