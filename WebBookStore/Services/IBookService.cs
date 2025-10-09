using System.Collections.Generic;
using WebBookStore.Models;

namespace WebBookStore.Services
{
    public interface IBookService
    {
        // Get operations
        Book GetBookById(int id);
        Book GetBookWithDetails(int id);
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> GetBooksByCategory(int categoryId);
        IEnumerable<Book> GetBestSellingBooks(int count = 8);
        IEnumerable<Book> GetNewBooks(int count = 8);
        IEnumerable<Book> GetDiscountedBooks();
        IEnumerable<Book> SearchBooks(string searchTerm);

        // CRUD operations
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);

        // Stock management
        bool IsInStock(int bookId, int quantity);
        void UpdateStock(int bookId, int quantity);
    }
}