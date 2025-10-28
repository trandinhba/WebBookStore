using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebBookStore.Services;
using WebBookStore.Data;
using WebBookStore.Repositories;
using WebBookStore.ViewModels;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IBookService _bookService;

        public CartController() : this(
            new CartService(new StoreDbContext()),
            new BookService(new BookRepository(new StoreDbContext()), new StoreDbContext()))
        {
        }

        public CartController(ICartService cartService, IBookService bookService)
        {
            _cartService = cartService;
            _bookService = bookService;
        }

        // GET: Cart
        [Authorize]
        public ActionResult Index()
        {
            var userId = GetCurrentUserId();
            var cart = _cartService.GetUserCart(userId);

            var viewModel = new CartViewModel();
            
            if (cart != null && cart.CartItems != null)
            {
                foreach (var item in cart.CartItems)
                {
                    var book = _bookService.GetBookById(item.BookId);
                    if (book != null)
                    {
                        var unitPrice = book.DiscountPrice ?? book.Price;
                        var cartItem = new CartItemViewModel
                        {
                            CartItemId = item.CartItemId,
                            CartId = item.CartId,
                            BookId = item.BookId,
                            Book = book,
                            Quantity = item.Quantity,
                            UnitPrice = unitPrice,
                            TotalPrice = unitPrice * item.Quantity
                        };
                        
                        viewModel.Items.Add(cartItem);
                        viewModel.SubTotal += cartItem.TotalPrice;
                    }
                }
            }

            viewModel.TotalAmount = viewModel.SubTotal; // No shipping fee for now
            viewModel.TotalItems = viewModel.Items.Sum(i => i.Quantity);

            return View(viewModel);
        }

        // POST: Cart/AddToCart
        [HttpPost]
        public JsonResult AddToCart(int bookId, int quantity = 1)
        {
            var userId = GetCurrentUserId();

            if (userId == 0)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để thêm vào giỏ hàng" });
            }

            var book = _bookService.GetBookById(bookId);

            if (book == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
            }

            if (!_bookService.IsInStock(bookId, quantity))
            {
                return Json(new { success = false, message = "Sản phẩm không đủ số lượng trong kho" });
            }

            _cartService.AddToCart(userId, bookId, quantity);

            var cartCount = _cartService.GetCartItemCount(userId);

            return Json(new { success = true, message = "Đã thêm vào giỏ hàng", cartCount = cartCount });
        }

        // POST: Cart/UpdateQuantity
        [HttpPost]
        [Authorize]
        public JsonResult UpdateQuantity(int cartItemId, int quantity)
        {
            var userId = GetCurrentUserId();

            _cartService.UpdateCartItem(userId, cartItemId, quantity);

            var cart = _cartService.GetUserCart(userId);

            return Json(new
            {
                success = true,
                cartTotal = cart.TotalAmount,
                cartCount = cart.TotalItems
            });
        }

        // POST: Cart/RemoveItem
        [HttpPost]
        [Authorize]
        public JsonResult RemoveItem(int cartItemId)
        {
            var userId = GetCurrentUserId();

            _cartService.RemoveFromCart(userId, cartItemId);

            var cart = _cartService.GetUserCart(userId);

            return Json(new
            {
                success = true,
                message = "Đã xóa sản phẩm khỏi giỏ hàng",
                cartTotal = cart.TotalAmount,
                cartCount = cart.TotalItems
            });
        }

        // GET: Cart/GetCartCount
        public JsonResult GetCartCount()
        {
            var userId = GetCurrentUserId();

            if (userId == 0)
            {
                return Json(new { count = 0 }, JsonRequestBehavior.AllowGet);
            }

            var count = _cartService.GetCartItemCount(userId);

            return Json(new { count = count }, JsonRequestBehavior.AllowGet);
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