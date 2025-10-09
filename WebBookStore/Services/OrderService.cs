using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebBookStore.Data;
using WebBookStore.Models;

namespace WebBookStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly StoreDbContext _context;
        private readonly ICartService _cartService;

        public OrderService(StoreDbContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public Order CreateOrder(int userId, Order orderInfo)
        {
            var cart = _cartService.GetUserCart(userId);

            if (cart.CartItems.Count == 0)
                throw new InvalidOperationException("Giỏ hàng trống");

            var order = new Order
            {
                UserId = userId,
                ShippingAddress = orderInfo.ShippingAddress,
                PaymentMethod = orderInfo.PaymentMethod,
                Subtotal = 0,
                DiscountAmount = 0,
                Total = 0,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Items = new List<OrderItem>()
            };

            foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    BookId = cartItem.BookId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Book.FinalPrice,
                    Discount = 0
                };
                order.Items.Add(orderItem);

                // Update stock
                var book = _context.Books.Find(cartItem.BookId);
                if (book != null)
                {
                    book.StockQuantity -= cartItem.Quantity;
                }
                order.Subtotal += orderItem.UnitPrice * orderItem.Quantity;
            }

            order.Total = order.Subtotal - order.DiscountAmount;

            _context.Orders.Add(order);
            _cartService.ClearCart(userId);
            _context.SaveChanges();

            return order;
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders
                .Include(o => o.Items.Select(oi => oi.Book))
                .FirstOrDefault(o => o.Id == orderId);
        }

        public IEnumerable<Order> GetUserOrders(int userId)
        {
            return _context.Orders
                .Include(o => o.Items.Select(oi => oi.Book))
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), status, true);
                order.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
            }
        }

        public void UpdatePaymentStatus(int orderId, string paymentStatus)
        {
            // No payment status on model; method kept for compatibility
        }

        public bool ApplyDiscount(int orderId, string discountCode)
        {
            // No Discount aggregate implemented in model; not supported currently
            return false;
        }
    }
}