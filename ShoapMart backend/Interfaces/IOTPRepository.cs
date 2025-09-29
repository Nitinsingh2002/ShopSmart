namespace ShopMart.Api.Interfaces
{
    public interface IOTPRepository
    {
        Task<string> GenerateOTPAsync(string phoneNumber);


        Task<bool> SendOTPAsync(string phoneNumber, string otp, string name);
    }
}