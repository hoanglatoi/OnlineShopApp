using OnlineShop.Model.Models;
using System.Security.Claims;

namespace OnlineShop.Service.Services.Token
{
    public interface ITokenManager
    {
        public string GenerateAccessToken(ApplicationUser user, ApplicationGroup group);
        public RefreshToken GenerateRefreshToken(string userName);
    }
}
