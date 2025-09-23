namespace ShopMart.Api.Entities
{
    public class Vendor
    {
        public int Id { get; set; }
        public string ShoapName { get; set; } = string.Empty;
        public bool IsVendorVerified { get; set; }

        // Foreign key to ApplicationUser
        public string UserId { get; set; } = string.Empty;
        public  ApplicationUser User { get; set; } = null!;

    }
}