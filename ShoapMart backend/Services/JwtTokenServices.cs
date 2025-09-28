using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ShopMart.Api.Entities;
using ShopMart.Api.Interfaces;

namespace ShopMart.Api.Services
{
    public class JwtTokenServices : ITokenRepository
    {
        private readonly IConfiguration _Configuration;
        public JwtTokenServices(IConfiguration _Configuration)
        {
            this._Configuration = _Configuration;
        }
        public string createToken(ApplicationUser user, string role)
        {
            var claim = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
               new Claim(ClaimTypes.Role,role),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["jwt:Key"]));
            var Credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                 issuer: _Configuration["jwt:Issuer"],
                audience: _Configuration["jwt:Audience"],
                  claims: claim,
                  expires: DateTime.Now.AddDays(1),
                  signingCredentials: Credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}