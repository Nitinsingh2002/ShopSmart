using ShopMart.Api.Entities;

namespace ShopMart.Api.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;

        // here we have to add default address and all address latter
    }
}