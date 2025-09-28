using ShopMart.Api.Entities;

namespace ShopMart.Api.Interfaces
{
    public interface ITokenRepository
    {
        public string createToken(ApplicationUser user, string role);
    }
}