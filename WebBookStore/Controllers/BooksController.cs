using System.Linq;
using System.Web.Mvc;
using WebBookStore.Data;
using WebBookStore.Services;
using WebBookStore.Repositories;

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

            // Search
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                books = books.Where(b => b.Title.Contains(searchTerm) ||
                                        b.Author.Contains(searchTerm) ||
                                        b.ISBN.Contains(searchTerm));
            }

            // Sort
            switch (sortBy)
            {
                case "price_asc":
                    books = books.OrderBy(b => b.FinalPrice);
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.FinalPrice);
                    break;
                case "name_asc":
                    books = books.OrderBy(b => b.Title);
                    break;
                case "name_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "newest":
                    books = books.OrderByDescending(b => b.CreatedDate);
                    break;
                default:
                    books = books.OrderByDescending(b => b.CreatedDate);
                    break;
            }

            // Pagination
            var totalItems = books.Count();
            var totalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);
            var pagedBooks = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.CurrentCategory = categoryId;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortBy = sortBy;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

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

            return View(books);
        }

        // POST: Books/AddReview
        [HttpPost]
        [Authorize]
        public ActionResult AddReview(int bookId, int rating, string comment)
        {
            var userId = GetCurrentUserId();

            var review = new Models.Review
            {
                BookId = bookId,
                UserId = userId,
                Rating = rating,
                Comment = comment
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = bookId });
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