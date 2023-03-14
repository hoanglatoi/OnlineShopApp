namespace OnlineShop.AppStart
{
    public class LoggerConfig
    {
        public static void RegisterLoggers(ref WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Logging.AddLog4Net();
        }
    }
}
