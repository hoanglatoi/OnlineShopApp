using Microsoft.EntityFrameworkCore;

namespace OnlineShop.AppStart
{
    public class DbContextConfig
    {
        public static void RegisterDbContext(ref WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<OnlineShop.Data.ShopOnlineDbContext>(options => options
            .UseNpgsql(connectionString));
            //builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        }
    }
}
