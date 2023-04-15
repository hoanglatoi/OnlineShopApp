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
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.PostContents == null)
            {
                return NotFound();
            }

            var PostItem = await _context.PostContents.FindAsync(id);
            if (PostItem == null)
            {
                return NotFound();
            }
            return View(PostItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,Tags,MetaTitle,ViewCount,Detail,CategoryID,Warranty,MetaDescription")] PostContent PostItem)
        {
            if (id != PostItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(PostItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostContentExists(PostItem.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(PostItem);
        }
        public async Task<IActionResult> ViewDetails(long? id)
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
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.PostContents == null)
            {
                return NotFound();
            }

            var PostItem = await _context.PostContents
                .FirstOrDefaultAsync(m => m.ID == id);
            if (PostItem == null)
            {
                return NotFound();
            }

            return View(PostItem);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.PostContents == null)
            {
                return Problem("Entity set 'OnlineShopPostContext.Item'  is null.");
            }
            var postItem = await _context.PostContents.FindAsync(id);
            if (postItem != null)
            {
                _context.PostContents.Remove(postItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool PostContentExists(long id)
        {
            return (_context.PostContents?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
