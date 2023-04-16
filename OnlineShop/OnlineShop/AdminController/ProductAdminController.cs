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
            var ProductCategoriesList = from m in _context.ProductCategories select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductCategoriesList = ProductCategoriesList.Where(s => s.Name!.Contains(searchString));
            }

            return View(await ProductCategoriesList.ToListAsync());
        }
        public async Task<IActionResult> IndexProducts(string id, string searchString)
        {
            var ProductList = from m in _context.Products select m;

            ProductList = ProductList.Where(s => s.CategoryName!.Contains(id));

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductList = ProductList.Where(s => s.Name!.Contains(searchString));
            }

            return View(await ProductList.ToListAsync());
        }
    }
}
