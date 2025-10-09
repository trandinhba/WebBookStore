using System;
using System.Collections.Generic;
using WebBookStore.Data;
using WebBookStore.Models;
using WebBookStore.Repositories;

namespace WebBookStore.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly StoreDbContext _context;

        public BookService(IBookRepository bookRepository, StoreDbContext context)
        {
            _bookRepository = bookRepository;
            _context = context;
        }

        public Book GetBookById(int id)
        {
            return _bookRepository.GetById(id);
        }

        public Book GetBookWithDetails(int id)
        {
            return _bookRepository.GetBookWithDetails(id);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.Find(b => b.IsActive);
        }

        public IEnumerable<Book> GetBooksByCategory(int categoryId)
        {
            return _bookRepository.GetBooksByCategory(categoryId);
        }

        public IEnumerable<Book> GetBestSellingBooks(int count = 8)
        {
            return _bookRepository.GetBestSellingBooks(count);
        }

        public IEnumerable<Book> GetNewBooks(int count = 8)
        {
            return _bookRepository.GetNewBooks(count);
        }

        public IEnumerable<Book> GetDiscountedBooks()
        {
            return _bookRepository.GetDiscountedBooks();
        }

        public IEnumerable<Book> SearchBooks(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Book>();

            return _bookRepository.SearchBooks(searchTerm);
        }

        public void AddBook(Book book)
        {
            _bookRepository.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            _bookRepository.Update(book);
            _context.SaveChanges();
        }

        public void DeleteBook(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book != null)
            {
                book.IsActive = false;
                _bookRepository.Update(book);
                _context.SaveChanges();
            }
        }

        public bool IsInStock(int bookId, int quantity)
        {
            var book = _bookRepository.GetById(bookId);
            return book != null && book.StockQuantity >= quantity;
        }

        public void UpdateStock(int bookId, int quantity)
        {
            var book = _bookRepository.GetById(bookId);
            if (book != null)
            {
                book.StockQuantity += quantity;
                _bookRepository.Update(book);
                _context.SaveChanges();
            }
        }
    }
}