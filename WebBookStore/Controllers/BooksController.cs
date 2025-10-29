using System;
using System.Linq;
using System.Web.Mvc;
using WebBookStore.Data;
using WebBookStore.Services;
using WebBookStore.Repositories;
using WebBookStore.Filters;
using WebBookStore.Helpers;

namespace WebBookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly StoreDbContext _context;

        public BooksController() : this(
            new BookService(new BookRepository(new StoreDbContext()), new StoreDbContext()),
            new StoreDbContext())
        {
        }

        public BooksController(IBookService bookService, StoreDbContext context)
        {
            _bookService = bookService;
            _context = context;
        }

        // GET: Books
        public ActionResult Index(int? categoryId, string searchTerm, string sortBy, int page = 1, int pageSize = 12)
        {
            var books = _bookService.GetAllBooks().AsQueryable();

            // Filter by category
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                books = books.Where(b => b.CategoryId == categoryId.Value);
            }

            // Convert to list first to avoid LINQ to SQL issues with computed properties and search
            var booksList = books.ToList();
            
            // Search (case-insensitive) - now in memory
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchLower = searchTerm.ToLower();
                booksList = booksList.Where(b => (b.Title != null && b.Title.ToLower().Contains(searchLower)) ||
                                                (b.Author != null && b.Author.ToLower().Contains(searchLower)) ||
                                                (b.ISBN != null && b.ISBN.ToLower().Contains(searchLower))).ToList();
            }
            
            // Sort in memory (for computed properties like FinalPrice)
            switch (sortBy)
            {
                case "price_asc":
                    booksList = booksList.OrderBy(b => b.FinalPrice).ToList();
                    break;
                case "price_desc":
                    booksList = booksList.OrderByDescending(b => b.FinalPrice).ToList();
                    break;
                case "name_asc":
                    booksList = booksList.OrderBy(b => b.Title).ToList();
                    break;
                case "name_desc":
                    booksList = booksList.OrderByDescending(b => b.Title).ToList();
                    break;
                case "newest":
                    booksList = booksList.OrderByDescending(b => b.CreatedDate).ToList();
                    break;
                default:
                    booksList = booksList.OrderByDescending(b => b.CreatedDate).ToList();
                    break;
            }

            // Pagination
            var totalItems = booksList.Count();
            var totalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);
            var pagedBooks = booksList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.CurrentCategory = categoryId;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortBy = sortBy;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();

            return View(pagedBooks);
        }

        // GET: Books/Details/5
        public ActionResult Details(int id)
        {
            var book = _bookService.GetBookWithDetails(id);

            if (book == null)
            {
                return HttpNotFound();
            }

            // Get related books from same category
            var relatedBooks = _bookService.GetBooksByCategory(book.CategoryId)
                .Where(b => b.BookId != id)
                .Take(4)
                .ToList();

            ViewBag.RelatedBooks = relatedBooks;
            ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();

            return View(book);
        }

        // GET: Books/Category/5
        public ActionResult Category(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var books = _bookService.GetBooksByCategory(id);
            ViewBag.Category = category;
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();

            return View(books);
        }

        // POST: Books/AddReview
        [HttpPost]
        [CustomerOnly]
        public ActionResult AddReview(int bookId, int rating, string comment)
        {
            try
            {
                var userId = GetCurrentUserId();

                if (userId == 0)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để đánh giá" });
                }

                var review = new Models.Review
                {
                    BookId = bookId,
                    UserId = userId,
                    Rating = rating,
                    Comment = comment,
                    CreatedDate = DateTime.Now,
                    ReviewDate = DateTime.Now
                };

                _context.Reviews.Add(review);
                _context.SaveChanges();

                return Json(new { success = true, message = "Cảm ơn bạn đã đánh giá!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
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