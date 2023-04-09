using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.AdminController
{
    public class ProductAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
