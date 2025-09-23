using ShopMart.Api.Entities;

namespace ShopMart.Api.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public DateTime? DateOfBirth { get; set; }

        //foreign key
        public string UserId { get; set; } = string.Empty;
        public  ApplicationUser User { get; set; } = null!;

        // here we have to add default address and all address latter
    }
}