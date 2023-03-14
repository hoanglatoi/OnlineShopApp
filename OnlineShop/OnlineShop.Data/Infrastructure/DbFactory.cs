
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OnlineShop.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private ShopOnlineDbContext? dbContext;

        public DbFactory( ShopOnlineDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ShopOnlineDbContext Init()
        {
            if(dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            return dbContext;
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}