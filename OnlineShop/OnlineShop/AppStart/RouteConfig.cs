namespace OnlineShop.AppStart
{
    public class RouteConfig
    {
        public static void RegisterRoutes(ref WebApplication app)
        {
            app.MapControllerRoute(
                name: "learnasproute",  
                defaults: new { controller = "LearnAsp", action = "Index" },
                pattern: "learn-asp-net/{id:int?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
        }
    }
}
