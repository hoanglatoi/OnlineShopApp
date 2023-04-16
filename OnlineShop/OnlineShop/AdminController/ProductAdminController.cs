using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Model.Models;
using Microsoft.EntityFrameworkCore;


namespace OnlineShop.AdminController
{
    public class ProductAdminController : Controller
    {
        private readonly ShopOnlineDbContext _context;
        public ProductAdminController(ShopOnlineDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            var ProductList = from m in _context.Products
                                  select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductList = ProductList.Where(s => s.Name!.Contains(searchString));
            }

            return View(await ProductList.ToListAsync());
        }
    }
}
