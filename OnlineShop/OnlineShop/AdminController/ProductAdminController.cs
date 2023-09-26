using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Model.Models;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Service.Services.FileExcute;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.ViewModels;

namespace OnlineShop.AdminController
{
    public class ProductAdminController : Controller
    {
        private readonly ShopOnlineDbContext _context;
        public ProductAdminController(ShopOnlineDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index(string searchString)
        {
            var ProductCategoriesList = from m in _context.ProductCategories select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductCategoriesList = ProductCategoriesList.Where(s => s.Name!.Contains(searchString));
            }

            List<ProductCategoryVM> productCategoryVMList = new List<ProductCategoryVM>();

            foreach (ProductCategory item in await ProductCategoriesList.ToListAsync())
            {
                ProductCategoryVM productCategoryVM = AutoMap.Instance!.Mapper.Map<ProductCategoryVM>(item);
                productCategoryVMList.Add(productCategoryVM);
            }

            return View(productCategoryVMList);
        }

        [Authorize]
        public async Task<IActionResult> IndexCategory()
        {
            var ProductCategoriesList = from m in _context.ProductCategories select m;

            List<ProductCategoryVM> productCategoryVMList = new List<ProductCategoryVM>();

            foreach (ProductCategory item in await ProductCategoriesList.ToListAsync())
            {
                ProductCategoryVM productCategoryVM = AutoMap.Instance!.Mapper.Map<ProductCategoryVM>(item);
                productCategoryVMList.Add(productCategoryVM);
            }

            return View(productCategoryVMList);
        }

        [Authorize]
        public IActionResult IndexProducts(string id, string searchString)
        {
            var ProductList = from m in _context.Products select m;

            ProductList = ProductList.Where(s => s.CategoryName!.Contains(id));

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductList = ProductList.Where(s => s.Name!.Contains(searchString));
            }

            List<ProductVM> ProductVMList = new List<ProductVM>();

            foreach(Product item in ProductList)
            {
                ProductVM productVM = AutoMap.Instance!.Mapper.Map<ProductVM>(item);
                ProductVMList.Add(productVM);
            }

            return View(ProductVMList);
        }

        [Authorize]
        public  IActionResult IndexProductNew(string id, string searchString)
        {
            var ProductList = from m in _context.Products select m;

            ProductList = ProductList.Where(s => s.CategoryName!.Contains(id));

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductList = ProductList.Where(s => s.Name!.Contains(searchString));
            }

            List<ProductVM> ProductVMList = new List<ProductVM>();

            foreach (Product item in ProductList)
            {
                ProductVM productVM = AutoMap.Instance!.Mapper.Map<ProductVM>(item);
                ProductVMList.Add(productVM);
            }

            return View(ProductVMList);
        }

        [Authorize]
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

            ProductVM productVM = AutoMap.Instance!.Mapper.Map<ProductVM>(ProductItem);

            return View(productVM);
        }

        [Authorize]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([Bind("Name,ParentID,MetaDescription")] ProductCategoryVM productCategory)
        {
            if (ModelState.IsValid)
            {
                ProductCategory Item = AutoMap.Instance!.Mapper.Map<ProductCategory>(productCategory);
                Item.Status = true;
                _context.Add(Item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexCategory));
            }

            return View(productCategory);
        }

        [Authorize]
        public async Task<IActionResult> CreateProduct(long? Id)
        {
            var ItemVM = new ProductVM();
            var Product_Item = await _context.Products.FirstOrDefaultAsync(m => m.CategoryID == Id);
            ItemVM.CategoryID = Id;
            if (Product_Item != null)
            {
                ItemVM.Name = Product_Item.CategoryName;
            }
            return View(ItemVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(long? Id, [Bind("Name,Code,Price,PromotionPrice,Quantity,CategoryID,Warranty,Name,Image")] ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                Product Item = AutoMap.Instance!.Mapper.Map<Product>(productVM);
                Item.Status = true;
                _context.Add(Item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexProducts), new { id = Item.CategoryName });
            }
            return View(productVM);
        }

        [Authorize]
        public async Task<IActionResult> EditProduct(long? Id)
        {
            if (Id == null || _context.Products == null)
            {
                return NotFound();
            }

            var Product_Item = await _context.Products.FindAsync(Id);
            if (Product_Item == null)
            {
                return NotFound();
            }

            ProductVM ItemVM = AutoMap.Instance!.Mapper.Map<ProductVM>(Product_Item);

            return View(ItemVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProduct(long? Id, [Bind("ID,Name,Code,Price,PromotionPrice,Quantity,CategoryID,Warranty,CategoryName,Image")] ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product Item = AutoMap.Instance!.Mapper.Map<Product>(productVM);
                    Item.Status = true;
                    _context.Update(Item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(IndexProducts), new { id = productVM.CategoryName });
            }
            return View(productVM);
        }

        [Authorize]
        public async Task<IActionResult> DeleteProduct(long? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var ProductItem = await _context.Products.FirstOrDefaultAsync(m => m.ID == id);
            if (ProductItem == null)
            {
                return NotFound();
            }

            ProductVM ItemVM = AutoMap.Instance!.Mapper.Map<ProductVM>(ProductItem);

            return View(ItemVM);
        }

        [Authorize]
        [HttpPost, ActionName("DeleteProduct")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'OnlineShopPostContext.Item'  is null.");
            }
            var postItem = await _context.Products.FindAsync(id);
            if (postItem != null)
            {
                _context.Products.Remove(postItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(IndexProducts), new { id = postItem!.CategoryName });
        }
    }
}
