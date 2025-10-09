using System.Collections.Generic;
using WebBookStore.Models;

namespace WebBookStore.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<Book> GetBooksByCategory(int categoryId);
        IEnumerable<Book> GetBestSellingBooks(int count);
        IEnumerable<Book> GetNewBooks(int count);
        IEnumerable<Book> GetDiscountedBooks();
        IEnumerable<Book> SearchBooks(string searchTerm);
        Book GetBookWithDetails(int bookId);
    }
}