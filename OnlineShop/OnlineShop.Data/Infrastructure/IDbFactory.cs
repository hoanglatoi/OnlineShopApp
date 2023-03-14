using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OnlineShop.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        ShopOnlineDbContext Init();
    }
}
