using System.ComponentModel.DataAnnotations;

namespace ShoapMart.Api.DTOs
{
    public class RegisterRequestDto
    {

        [Required]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters long.")]
        [StringLength(20, ErrorMessage = "First name cannot be longer than 20 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters long.")]
        [MaxLength(20, ErrorMessage = "Last name cannot be longer than 20 characters.")]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public DateTime? DateOfBirth { get; set; }

        [MinLength(2, ErrorMessage = "Shop name must be at least 2 characters long.")]
        [MaxLength(20, ErrorMessage = "Shop name cannot be longer than 20 characters.")]
        public string? ShopName { get; set; }

        public bool? IsVendorVerified { get; set; } 

    }
}