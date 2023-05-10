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
        public async Task<IActionResult> IndexCategories(string searchString)
        {
            var PostContentList = from m in _context.PostCategories
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                PostContentList = PostContentList.Where(s => s.Name!.Contains(searchString));
            }

            return View(await PostContentList.ToListAsync());
        }
        public async Task<IActionResult> IndexPost(long? id, string searchString)
        {
            var ProductList = from m in _context.PostContents select m;

            ProductList = ProductList.Where(s => s.CategoryID == id);

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductList = ProductList.Where(s => s.Name!.Contains(searchString));
            }
            ViewBag.Id = id;
            return View(await ProductList.ToListAsync());
        }

        public IActionResult CreateCategories()
        {
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> CreateCategories(PostCategory PostItem)
        {
            if (ModelState.IsValid)
            {
                PostItem.Status = true;
                _context.Add(PostItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexPost));
            }
            return View(PostItem);
        }

        public IActionResult CreatePost(long? Id)
        {
            var PostItem = new PostContent();
            PostItem.CategoryID = Id;
            PostItem.ViewCount = 0;
            return View(PostItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(long? Id, PostContent PostItem)
        {
            if (ModelState.IsValid)
            {
                PostItem.Status = true;
                _context.Add(PostItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexPost));
            }
            return View(PostItem);
        }
        public async Task<IActionResult> EditPost(long? id)
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
        public async Task<IActionResult> EditCategories(long? id)
        {
            if (id == null || _context.PostCategories == null)
            {
                return NotFound();
            }

            var PostCategoriesItem = await _context.PostCategories.FindAsync(id);
            if (PostCategoriesItem == null)
            {
                return NotFound();
            }
            return View(PostCategoriesItem);
        }
        [HttpPost]
        public async Task<IActionResult> EditCategories(long? id, PostCategory PostCatgItem)
        {
            if (id != PostCatgItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(PostCatgItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostContentExists(PostCatgItem.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexCategories));
            }
            return View(PostCatgItem);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(long id, PostContent PostItem)
        {
            if (id != PostItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PostItem.Status = true;
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
                return RedirectToAction(nameof(IndexPost));
            }
            return View(PostItem);
        }
        public async Task<IActionResult> ViewPostDetails(long? id)
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
        public async Task<IActionResult> DeletePost(long? id)
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

        [HttpPost, ActionName("DeletePost")]
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
            return RedirectToAction(nameof(IndexPost));
        }
        public async Task<IActionResult> DeleteCategories(long? id)
        {
            if (id == null || _context.PostContents == null)
            {
                return NotFound();
            }

            var PosttList = from m in _context.PostContents select m;
            PosttList = PosttList.Where(s => s.CategoryID == id);

            if (PosttList == null)
            {
                return NotFound();
            }

            return View(await PosttList.ToListAsync());
        }
        private bool PostContentExists(long id)
        {
            return (_context.PostContents?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
