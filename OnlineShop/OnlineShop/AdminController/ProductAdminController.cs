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


        public async Task<IActionResult> IndexCategory()
        {
            var ProductCategoriesList = from m in _context.ProductCategories select m;
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

        public async Task<IActionResult> IndexProductNew(string id, string searchString)
        {
            var ProductList = from m in _context.Products select m;

            ProductList = ProductList.Where(s => s.CategoryName!.Contains(id));

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductList = ProductList.Where(s => s.Name!.Contains(searchString));
            }

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
        public async Task<IActionResult> CreateProduct(long? Id)
        {
            var Item = new Product();
            var Product_Item = await _context.Products.FirstOrDefaultAsync(m => m.CategoryID == Id);
            Item.CategoryID = Id;
            if (Product_Item != null)
            {
                Item.CategoryName = Product_Item.CategoryName;
            }
            return View(Item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(long? Id, [Bind("Name,Code,Price,PromotionPrice,Quantity,CategoryID,Warranty,CategoryName,Image")] Product Item)
        {
            if (ModelState.IsValid)
            {
                Item.Status = true;
                _context.Add(Item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexProductNew), new { id = Item.CategoryName });
            }
            return View(Item);
        }
    }
}
