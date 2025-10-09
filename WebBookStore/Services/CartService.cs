using System.Data.Entity;
using System.Linq;
using WebBookStore.Data;
using WebBookStore.Models;

namespace WebBookStore.Services
{
    public class CartService : ICartService
    {
        private readonly StoreDbContext _context;

        public CartService(StoreDbContext context)
        {
            _context = context;
        }

        public Cart GetUserCart(int userId)
        {
            var cart = _context.Carts
                .Include(c => c.CartItems.Select(ci => ci.Book))
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            return cart;
        }

        public void AddToCart(int userId, int bookId, int quantity)
        {
            var cart = GetUserCart(userId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.BookId == bookId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    BookId = bookId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }

            _context.SaveChanges();
        }

        public void UpdateCartItem(int userId, int cartItemId, int quantity)
        {
            var cart = GetUserCart(userId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);

            if (cartItem != null)
            {
                if (quantity <= 0)
                {
                    _context.CartItems.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = quantity;
                }
                _context.SaveChanges();
            }
        }

        public void RemoveFromCart(int userId, int cartItemId)
        {
            var cart = GetUserCart(userId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                _context.SaveChanges();
            }
        }

        public void ClearCart(int userId)
        {
            var cart = GetUserCart(userId);
            _context.CartItems.RemoveRange(cart.CartItems);
            _context.SaveChanges();
        }

        public int GetCartItemCount(int userId)
        {
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId);

            return cart?.CartItems.Count ?? 0;
        }

        public decimal GetCartTotal(int userId)
        {
            var cart = GetUserCart(userId);
            return cart.TotalAmount;
        }
    }
}