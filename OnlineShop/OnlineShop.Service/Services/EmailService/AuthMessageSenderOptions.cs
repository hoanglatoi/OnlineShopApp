namespace OnlineShop.Service.Services.FileExcute
{
    public class AuthMessageSenderOptions
    {
        public List<string> MethodList { get; set; } = new List<string>();
        public string Method { get; set; } = string.Empty;
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 25;
        public string SmtpUserID { get; set; } = string.Empty;
        public string SmtpPass { get; set; } = string.Empty;
        public string? SendGridKey { get; set; }
    }
}
