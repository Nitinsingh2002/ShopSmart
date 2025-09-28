using ShopMart.Api.Enums;

namespace ShoapMart.Api.DTOs
{
    public class LoginRequestDTO
    {
        public string? Email { get; set; }
        public LoginType  LoginMethod { get; set; } = LoginType.Email;
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}