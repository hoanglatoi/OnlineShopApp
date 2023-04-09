using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.AdminController
{
    public class PostAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
