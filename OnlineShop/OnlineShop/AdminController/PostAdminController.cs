using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.AdminController
{
    public class PostAdminController : Controller
    {
        private readonly ShopOnlineDbContext _context;
        public PostAdminController(ShopOnlineDbContext context) {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            var PostContentList = from m in _context.PostContents
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                PostContentList = PostContentList.Where(s => s.Name!.Contains(searchString));
            }

            return View(await PostContentList.ToListAsync());
        }

        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Tags,MetaTitle,ViewCount,Detail,CategoryID,Warranty,MetaDescription")] PostContent PostItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(PostItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(PostItem);
        }
        public async Task<IActionResult> ViewDetails(int? id)
        {
            if (id == null || _context.PostContents == null)
            {
                return NotFound();
            }

            var PostContent_Item = await _context.PostContents
                .FirstOrDefaultAsync(m => m.ID == id);
            if (PostContent_Item == null)
            {
                return NotFound();
            }

            return View(PostContent_Item);
        }
    }
}
