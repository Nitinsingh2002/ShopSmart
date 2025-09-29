using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using ShopMart.Api.Interfaces;

namespace ShoapMart.Api.Services
{
    public class OTPService : IOTPRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public OTPService(HttpClient _httpClient, IConfiguration _config)
        {
            this._httpClient = _httpClient;
            this._config = _config;
        }

        public async Task<string> GenerateOTPAsync(string phoneNumber)
        {
            var otp = new Random().Next(1000, 9999).ToString();
            return await Task.FromResult(otp);
        }


        public async Task<bool> SendOTPAsync(string email, string otp, string name)
        {
            var smtpHost = _config["Email:SmtpHost"];
            var smtpPort = int.Parse(_config["Email:SmtpPort"]);
            var smtpUser = _config["Email:SmtpUser"];
            var smtpPass = _config["Email:SmtpPass"];

            var mail = new MailMessage();
            mail.From = new MailAddress(smtpUser, "ShopMart Support");
            mail.To.Add(email);
            mail.Subject = "Your OTP for ShopMart Login";
            mail.IsBodyHtml = true;

            mail.Body = $@"
        <html>
        <body>
            <p>Dear {name},</p>
            <p>Your OTP for ShopMart login is: <b>{otp}</b></p>
            <p>This OTP is valid for 5 minutes. Do not share it with anyone.</p>
            <br/>
            <p>Thanks,<br/>ShopMart Team</p>
        </body>
        </html>";

            using var smtp = new SmtpClient(smtpHost, smtpPort);
            smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
            smtp.EnableSsl = true;

            await smtp.SendMailAsync(mail);
            return true;
        }
    }
}