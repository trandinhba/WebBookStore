using System.Web.Mvc;
using WebBookStore.Models;
using WebBookStore.Services;
using WebBookStore.Data;
using WebBookStore.Repositories;
using WebBookStore.Filters;
using WebBookStore.Helpers;

namespace WebBookStore.Controllers
{
    [CustomerOnly]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IEmailService _emailService;

        public OrderController() : this(
            new OrderService(new StoreDbContext(), new CartService(new StoreDbContext())),
            new CartService(new StoreDbContext()),
            new EmailService())
        {
        }

        public OrderController(IOrderService orderService, ICartService cartService, IEmailService emailService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _emailService = emailService;
        }

        // GET: Order/Checkout
        public ActionResult Checkout()
        {
            var userId = GetCurrentUserId();
            var cart = _cartService.GetUserCart(userId);

            if (cart.CartItems.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống";
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.Cart = cart;
            ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
            return View(new Order());
        }

        // POST: Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(Order order)
        {
            var userId = GetCurrentUserId();

            if (!ModelState.IsValid)
            {
                var cart = _cartService.GetUserCart(userId);
                ViewBag.Cart = cart;
                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                return View(order);
            }

            try
            {
                var createdOrder = _orderService.CreateOrder(userId, order);

                // Send confirmation email to customer
                _emailService.SendOrderConfirmation(createdOrder.Id);

                // Send invoice to admin
                _emailService.SendInvoiceToAdmin(createdOrder.Id);

                // Check if this is an AJAX request
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true, orderId = createdOrder.Id, message = "Đặt hàng thành công!" });
                }

                return RedirectToAction("Confirmation", new { id = createdOrder.Id });
            }
            catch (System.Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = false, message = ex.Message });
                }

                ModelState.AddModelError("", ex.Message);
                var cart = _cartService.GetUserCart(userId);
                ViewBag.Cart = cart;
                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                return View(order);
            }
        }

        // GET: Order/Confirmation/5
        public ActionResult Confirmation(int id)
        {
            var userId = GetCurrentUserId();
            var order = _orderService.GetOrderById(id);

            if (order == null || order.UserId != userId)
            {
                return HttpNotFound();
            }

            ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
            return View(order);
        }

        // GET: Order/History
        public ActionResult History()
        {
            var userId = GetCurrentUserId();
            var orders = _orderService.GetUserOrders(userId);

            return View(orders);
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            var userId = GetCurrentUserId();
            var order = _orderService.GetOrderById(id);

            if (order == null || order.UserId != userId)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // POST: Order/ApplyDiscount
        [HttpPost]
        public JsonResult ApplyDiscount(int orderId, string discountCode)
        {
            var userId = GetCurrentUserId();
            var order = _orderService.GetOrderById(orderId);

            if (order == null || order.UserId != userId)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
            }

            var success = _orderService.ApplyDiscount(orderId, discountCode);

            if (success)
            {
                var updatedOrder = _orderService.GetOrderById(orderId);
                return Json(new
                {
                    success = true,
                    message = "Áp dụng mã giảm giá thành công",
                    discountAmount = updatedOrder.DiscountAmount,
                    totalAmount = updatedOrder.Total
                });
            }

            return Json(new { success = false, message = "Mã giảm giá không hợp lệ hoặc đã hết hạn" });
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