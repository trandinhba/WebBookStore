using WebBookStore.Models;

namespace WebBookStore.Services
{
    public interface ICartService
    {
        Cart GetUserCart(int userId);
        void AddToCart(int userId, int bookId, int quantity);
        void UpdateCartItem(int userId, int cartItemId, int quantity);
        void RemoveFromCart(int userId, int cartItemId);
        void ClearCart(int userId);
        int GetCartItemCount(int userId);
        decimal GetCartTotal(int userId);
    }
}