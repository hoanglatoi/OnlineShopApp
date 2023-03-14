using Microsoft.AspNetCore.Identity;
using OnlineShop.Data;
using System.Reflection;
using OnlineShop.AppStart;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;

// Config log4net
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
log4net.ILog _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
_logger.Info("Application - Main is invoked");

var builder = WebApplication.CreateBuilder(args);

// Logger config
LoggerConfig.RegisterLoggers(ref builder);

// Add services to the container.
DbContextConfig.RegisterDbContext(ref builder);
IdentityConfig.RegisterIdentity(ref builder);
ServiceConfig.RegisterServices(ref builder);

var app = builder.Build();

// Identity Init
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    if (builder.Configuration.GetSection("Initialization:DBMigrate").Get<bool>())
    {
        var context = services.GetRequiredService<OnlineShop.Data.ShopOnlineDbContext>();
        context.Database.Migrate();
    }
    if (builder.Configuration.GetSection("Initialization:DBDataInit").Get<bool>())
    {
        var initData = new IdentityUserInitializer(services);
        initData.Initialize().Wait();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Use a reverse proxy server(nginx)
// https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-7.0
// https://qiita.com/wasimaru/items/4b160b48dae7f2618074
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("NgOrigins");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

RouteConfig.RegisterRoutes(ref app);
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

app.Run();
