using System.Security.Claims;

namespace OnlineShop.Service.Services.UserService
{
    public interface IUserService
    {
        string GetMyName();

        List<Claim> GetClaims();

        string GetClaimWithClaimType(string claimType);
    }
}
