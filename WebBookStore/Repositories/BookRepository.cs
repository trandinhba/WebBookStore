using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebBookStore.Data;
using WebBookStore.Models;

namespace WebBookStore.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(StoreDbContext context) : base(context)
        {
        }

        public IEnumerable<Book> GetBooksByCategory(int categoryId)
        {
            return _dbSet
                .Where(b => b.CategoryId == categoryId && b.IsActive)
                .Include(b => b.Category)
                .OrderByDescending(b => b.CreatedDate)
                .ToList();
        }

        public IEnumerable<Book> GetBestSellingBooks(int count)
        {
            return _dbSet
                .Where(b => b.IsActive)
                .Include(b => b.Category)
                .OrderByDescending(b => b.OrderItems.Sum(oi => oi.Quantity))
                .Take(count)
                .ToList();
        }

        public IEnumerable<Book> GetNewBooks(int count)
        {
            return _dbSet
                .Where(b => b.IsActive)
                .Include(b => b.Category)
                .OrderByDescending(b => b.CreatedDate)
                .Take(count)
                .ToList();
        }

        public IEnumerable<Book> GetDiscountedBooks()
        {
            return _dbSet
                .Where(b => b.IsActive && b.DiscountPrice.HasValue && b.DiscountPrice < b.Price)
                .Include(b => b.Category)
                .OrderByDescending(b => b.Price - b.DiscountPrice)
                .ToList();
        }

        public IEnumerable<Book> SearchBooks(string searchTerm)
        {
            return _dbSet
                .Where(b => b.IsActive &&
                           (b.Title.Contains(searchTerm) ||
                            b.Author.Contains(searchTerm) ||
                            b.ISBN.Contains(searchTerm)))
                .Include(b => b.Category)
                .ToList();
        }

        public Book GetBookWithDetails(int bookId)
        {
            return _dbSet
                .Include(b => b.Category)
                .Include(b => b.Reviews.Select(r => r.User))
                .FirstOrDefault(b => b.BookId == bookId);
        }
    }
}