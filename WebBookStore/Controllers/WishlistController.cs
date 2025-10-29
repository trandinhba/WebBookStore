using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using WebBookStore.Data;
using WebBookStore.Models;
using WebBookStore.Repositories;
using WebBookStore.Filters;
using WebBookStore.Helpers;

namespace WebBookStore.Controllers
{
    [CustomerOnly]
    public class WishlistController : Controller
    {
        private readonly StoreDbContext _context;

        public WishlistController() : this(new StoreDbContext())
        {
        }

        public WishlistController(StoreDbContext context)
        {
            _context = context;
        }

        // GET: Wishlist
        public ActionResult Index()
        {
            var userId = GetCurrentUserId();
            var wishlists = _context.Wishlists
                .Where(w => w.UserId == userId)
                .Include("Book")
                .Include("Book.Category")
                .OrderByDescending(w => w.AddedDate)
                .ToList();

            ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
            return View(wishlists);
        }

        // POST: Wishlist/Add
        [HttpPost]
        public JsonResult Add(int bookId)
        {
            var userId = GetCurrentUserId();

            var exists = _context.Wishlists.Any(w => w.UserId == userId && w.BookId == bookId);

            if (exists)
            {
                return Json(new { success = false, message = "Sản phẩm đã có trong danh sách yêu thích" });
            }

            var wishlist = new Wishlist
            {
                UserId = userId,
                BookId = bookId,
                AddedDate = DateTime.Now
            };

            _context.Wishlists.Add(wishlist);
            _context.SaveChanges();

            return Json(new { success = true, message = "Đã thêm vào danh sách yêu thích" });
        }

        // POST: Wishlist/Remove
        [HttpPost]
        public JsonResult Remove(int wishlistId)
        {
            var userId = GetCurrentUserId();
            var wishlist = _context.Wishlists.FirstOrDefault(w => w.WishlistId == wishlistId && w.UserId == userId);

            if (wishlist == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
            }

            _context.Wishlists.Remove(wishlist);
            _context.SaveChanges();

            return Json(new { success = true, message = "Đã xóa khỏi danh sách yêu thích" });
        }

        private int GetCurrentUserId()
        {
            if (Session["UserId"] != null)
            {
                return (int)Session["UserId"];
            }
            return 0;
        }
    }
}