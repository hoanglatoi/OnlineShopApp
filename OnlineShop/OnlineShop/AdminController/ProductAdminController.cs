using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Model.Models;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Service.Services.FileExcute;


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
        public async Task<IActionResult> IndexProducts(string name, string searchString)
        {
            var ProductList = from m in _context.Products select m;

            ProductList = ProductList.Where(s => s.CategoryName!.Contains(name));

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductList = ProductList.Where(s => s.Name!.Contains(searchString));
            }

            string path = "wwwroot/img/testProduct/thuoc1.png";
            byte[] imageData = System.IO.File.ReadAllBytes(path);
            string base64String = Convert.ToBase64String(imageData);

            ViewBag.ImageData = base64String;

            return View(await ProductList.ToListAsync());
        }
        public async Task<IActionResult> ViewProductDetails(long? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var ProductItem = await _context.Products.FindAsync(id);

            if (ProductItem == null)
            {
                return NotFound();
            }
            return View(ProductItem);
        }
        [HttpPost]
        public async Task<IActionResult> Test()
        {
            List<Dictionary<string, object>> records = new List<Dictionary<string, object>>();
            var item = new Dictionary<string, object>();
            var formdata = Request.Form;
            string file = formdata["file"];
            string folder = formdata["folder"];

            Console.WriteLine(file);
            Console.WriteLine(folder);
            byte[] imageData = System.IO.File.ReadAllBytes(file);

            item["return"] = "OK";
            item["value"] = 1;

            records.Add(item);

            return Json(records);

        }
    }
}
