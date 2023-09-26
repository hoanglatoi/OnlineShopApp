using Microsoft.AspNetCore.Mvc;
using OnlineShop;
using OnlineShop.Data;
using OnlineShop.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.ViewModels;

namespace OnlineShop.AdminController
{
    public class PostAdminController : Controller
    {
        private readonly ShopOnlineDbContext _context;
        public PostAdminController(ShopOnlineDbContext context) {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> IndexCategories(string searchString)
        {
            var PostCategoryList = from m in _context.PostCategories select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                PostCategoryList = PostCategoryList.Where(s => s.Name!.Contains(searchString));
            }

            List<PostCategoryVM> postCategoryVMList = new List<PostCategoryVM>();
            
            foreach (PostCategory item in await PostCategoryList.ToListAsync())
            {
                PostCategoryVM postCategoryVM = AutoMap.Instance!.Mapper.Map<PostCategoryVM>(item);
                postCategoryVMList.Add(postCategoryVM);
            }

            return View(postCategoryVMList);
        }

        [Authorize]
        public async Task<IActionResult> IndexPost(long? id, string searchString)
        {
            var postList = from m in _context.PostContents select m;

            postList = postList.Where(s => s.CategoryID == id);

            if (!String.IsNullOrEmpty(searchString))
            {
                postList = postList.Where(s => s.Name!.Contains(searchString));
            }

            List<PostContentVM> postContentVMList = new List<PostContentVM>();

            foreach (PostContent item in await postList.ToListAsync())
            {
                PostContentVM postVMList = AutoMap.Instance!.Mapper.Map<PostContentVM>(item);
                postContentVMList.Add(postVMList);
            }

            ViewBag.Id = id;
            return View(postContentVMList);
        }

        public IActionResult CreateCategories()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCategories([Bind("Name, MetaTitle, MetaDescription, ParentID")]PostCategoryVM postCategoryVMItem)
        {
            if (ModelState.IsValid)
            {
                var postCategory = AutoMap.Instance!.Mapper.Map<PostCategory>(postCategoryVMItem);
                postCategory.Status = true;
                _context.Add(postCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexCategories));
            }

            return View(postCategoryVMItem);
        }

        [Authorize]
        public IActionResult CreatePost(long? Id)
        {
            var postVMItem = new PostContentVM();
            postVMItem.CategoryID = Id;
            postVMItem.ViewCount = 0;

            return View(postVMItem);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost(long? Id, [Bind("Name,MetaTitle,ViewCount,Tags,CategoryID,Warranty,MetaDescription,Description")]PostContentVM postVMItem)
        {
            if (ModelState.IsValid)
            {
                var postItem = AutoMap.Instance!.Mapper.Map<PostContent>(postVMItem);
                postItem.Status = true;
                _context.Add(postItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexPost), new {id = postVMItem.CategoryID});
            }
            return View(postVMItem);
        }

        [Authorize]
        public async Task<IActionResult> EditCategories(long? id)
        {
            if (id == null || _context.PostCategories == null)
            {
                return NotFound();
            }

            var postCategoriesItem = await _context.PostCategories.FindAsync(id);

            if (postCategoriesItem == null)
            {
                return NotFound();
            }
            else
            {
                return View(AutoMap.Instance!.Mapper.Map<PostCategoryVM>(postCategoriesItem));
            }
        }

        [Authorize]
        public async Task<IActionResult> EditPost(long? id)
        {
            if (id == null || _context.PostContents == null)
            {
                return NotFound();
            }

            var postItem = await _context.PostContents.FindAsync(id);
            if (postItem == null)
            {
                return NotFound();
            }
            else
            {
                return View(AutoMap.Instance!.Mapper.Map<PostContentVM>(postItem));
            } 
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditCategories(long? id, PostCategoryVM postCategoryVMItem)
        {
            if (id != postCategoryVMItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                PostCategory postCategory = AutoMap.Instance!.Mapper.Map<PostCategory>(postCategoryVMItem);
                try
                {
                    _context.Update(postCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostContentExists(postCategory.ID))
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

            return View(postCategoryVMItem);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditPost(long id, PostContentVM postVMItem)
        {
            if (id != postVMItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                PostContent postContent = AutoMap.Instance!.Mapper.Map<PostContent>(postVMItem);
                try
                {
                    postVMItem.Status = true;
                    _context.Update(postContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostContentExists(postContent.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexPost), new { id = postContent.CategoryID });
            }

            return View(postVMItem);
        }

        [Authorize]
        public async Task<IActionResult> DeletePost(long? id)
        {
            if (id == null || _context.PostContents == null)
            {
                return NotFound();
            }

            var postItem = await _context.PostContents.FirstOrDefaultAsync(m => m.ID == id);

            if (postItem == null)
            {
                return NotFound();
            }
            else
            {
                return View(AutoMap.Instance!.Mapper.Map<PostContentVM>(postItem));
            }
            
        }

        [Authorize]
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
            return RedirectToAction(nameof(IndexPost), new { id = postItem!.CategoryID });
        }

        [Authorize]
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

        [Authorize]
        public async Task<IActionResult> PreViewPost(long? id)
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
            else
            {
                ViewData["id"] = PostItem.CategoryID;
                ViewData["htmlcode"] = PostItem.Detail;
            }
            return View();
        }

        [Authorize]
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
        private bool PostContentExists(long id)
        {
            return (_context.PostContents?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
