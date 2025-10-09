using System.Collections.Generic;
using WebBookStore.Models;

namespace WebBookStore.Services
{
    public interface IOrderService
    {
        Order CreateOrder(int userId, Order orderInfo);
        Order GetOrderById(int orderId);
        IEnumerable<Order> GetUserOrders(int userId);
        IEnumerable<Order> GetAllOrders();
        void UpdateOrderStatus(int orderId, string status);
        void UpdatePaymentStatus(int orderId, string paymentStatus);
        bool ApplyDiscount(int orderId, string discountCode);
    }
}