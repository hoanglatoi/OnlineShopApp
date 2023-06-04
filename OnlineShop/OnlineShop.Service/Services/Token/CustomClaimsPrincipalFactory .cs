using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineShop.Model.Models;
using System.Security.Claims;

namespace OnlineShop.Service.Services.Token
{
    public class CustomClaimsPrincipalFactory :
        UserClaimsPrincipalFactory<ApplicationUser>
    {
        public CustomClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
                : base(userManager, optionsAccessor)
        {
        }

        // This method is called only when login. It means that "the drawback   
        // of calling the database with each HTTP request" never happen.  
        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            if (principal.Identity != null)
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(
                    new[] { new Claim("Tesssssssssssss", "ssssssssssssssss") });
            }

            //if (!string.IsNullOrEmpty(user.PhoneNumber))
            //{
            //    if (principal.Identity != null)
            //    {
            //        ((ClaimsIdentity)principal.Identity).AddClaims(
            //            new[] { new Claim("Phone", "ssssssssssssssss") });
            //    }
            //}

            return principal;
        }
    }
}