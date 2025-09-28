namespace ShoapMart.Api.DTOs
{
    public class LoginRequestDTO
    {
        public string? Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}