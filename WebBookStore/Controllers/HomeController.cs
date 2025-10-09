using System.Web.Mvc;
using WebBookStore.Services;
using WebBookStore.Data;
using WebBookStore.Repositories;

namespace WebBookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService _bookService;

        public HomeController() : this(
            new BookService(
                new BookRepository(new StoreDbContext()),
                new StoreDbContext()))
        {
        }

        public HomeController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: Home
        public ActionResult Index()
        {
            var bestSellingBooks = _bookService.GetBestSellingBooks(8);
            var newBooks = _bookService.GetNewBooks(8);

            ViewBag.BestSellingBooks = bestSellingBooks;
            ViewBag.NewBooks = newBooks;

            return View();
        }

        // GET: Home/Search
        public ActionResult Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return RedirectToAction("Index", "Books");
            }

            var books = _bookService.SearchBooks(term);
            ViewBag.SearchTerm = term;

            return View(books);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}