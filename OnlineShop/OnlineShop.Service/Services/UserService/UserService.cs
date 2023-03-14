using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace OnlineShop.Service.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetMyName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return result;
        }

        public List<Claim> GetClaims()
        {
            var result = new List<Claim>();
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.Claims.ToList();
            }
            return result;
        }

        public string GetClaimWithClaimType(string claimType)
        {
            var result = string.Empty;
            Claim? claim = null; 
            if (_httpContextAccessor.HttpContext != null)
            {
                claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == claimType);
            }
            if(claim != null)
            {
                result = claim.Value;
            }
            return result;
        }
    }
}
