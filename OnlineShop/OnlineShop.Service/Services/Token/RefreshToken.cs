namespace OnlineShop.Service.Services.Token
{
    public class RefreshToken
    {
        public string UserName { get; set; } = string.Empty;
        public string UserID { get; set; } = string.Empty;
        public string RoleID { get; set; } = String.Empty;
        public int GroupID { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
    }
}
