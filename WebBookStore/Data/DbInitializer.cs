using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebBookStore.Models;

namespace WebBookStore.Data
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<StoreDbContext>
    {
        protected override void Seed(StoreDbContext context)
        {
            // Seed Categories
            var categories = new List<Category>
            {
                new Category { CategoryName = "Light Novel", Description = "Tiểu thuyết nhẹ Nhật Bản" },
                new Category { CategoryName = "Manga", Description = "Truyện tranh Nhật Bản" },
                new Category { CategoryName = "Văn học", Description = "Sách văn học trong và ngoài nước" },
                new Category { CategoryName = "Kinh tế", Description = "Sách về kinh tế và kinh doanh" },
                new Category { CategoryName = "Kỹ năng sống", Description = "Sách phát triển bản thân" }
            };
            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();

            // Seed Users
            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    PasswordHash = "admin123", // In production, use proper password hashing
                    Email = "admin@sach50.vn",
                    FullName = "Quản trị viên",
                    PhoneNumber = "0123456789",
                    Role = "Admin",
                    IsActive = true
                },
                new User
                {
                    Username = "hirumi",
                    PasswordHash = "user123",
                    Email = "hirumi@example.com",
                    FullName = "Hirumi",
                    PhoneNumber = "0987654321",
                    Role = "Customer",
                    IsActive = true
                },
                new User
                {
                    Username = "nguyen",
                    PasswordHash = "user123",
                    Email = "nguyen@example.com",
                    FullName = "Nguyễn Văn A",
                    PhoneNumber = "0912345678",
                    Role = "Customer",
                    IsActive = true
                }
            };
            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            // Seed Books
            var books = new List<Book>
            {
                new Book
                {
                    Title = "Sword Art Online Progressive Vol 7",
                    Author = "REKI KAWAHARA",
                    Publisher = "IPM, Hà Nội",
                    PublishYear = 2022,
                    ISBN = "8935250707640",
                    Price = 120000,
                    DiscountPrice = 120000,
                    StockQuantity = 50,
                    ImageUrl = "/Content/images/books/sao-progressive-7.jpg",
                    Description = "Để Thấy Sword Art Online Có Không Gian Kể Chuyện Rất Rộng...",
                    Language = "Tiếng Việt",
                    PageCount = 360,
                    CategoryId = 1,
                    IsActive = true
                },
                new Book
                {
                    Title = "Sword Art Online Progressive Vol 1",
                    Author = "REKI KAWAHARA",
                    Publisher = "IPM, Hà Nội",
                    PublishYear = 2020,
                    ISBN = "8935250700001",
                    Price = 120000,
                    DiscountPrice = 120000,
                    StockQuantity = 30,
                    ImageUrl = "/Content/images/books/sao-progressive-1.jpg",
                    Description = "Khởi đầu câu chuyện Sword Art Online",
                    Language = "Tiếng Việt",
                    PageCount = 320,
                    CategoryId = 1,
                    IsActive = true
                },
                new Book
                {
                    Title = "Sword Art Online Progressive Vol 5",
                    Author = "REKI KAWAHARA",
                    Publisher = "IPM, Hà Nội",
                    PublishYear = 2021,
                    ISBN = "8935250705000",
                    Price = 120000,
                    DiscountPrice = 120000,
                    StockQuantity = 40,
                    ImageUrl = "/Content/images/books/sao-progressive-5.jpg",
                    Description = "Tiếp tục hành trình chinh phục Aincrad",
                    Language = "Tiếng Việt",
                    PageCount = 340,
                    CategoryId = 1,
                    IsActive = true
                },
                new Book
                {
                    Title = "Sword Art Online Vol 18",
                    Author = "REKI KAWAHARA",
                    Publisher = "IPM, Hà Nội",
                    PublishYear = 2022,
                    ISBN = "8935250718000",
                    Price = 100000,
                    DiscountPrice = 90000,
                    StockQuantity = 25,
                    ImageUrl = "/Content/images/books/sao-18.jpg",
                    Description = "Arc Alicization tiếp tục",
                    Language = "Tiếng Việt",
                    PageCount = 380,
                    CategoryId = 1,
                    IsActive = true
                },
                new Book
                {
                    Title = "Chú Thuật Hồi Chiến Tập 4",
                    Author = "GEGE AKUTAMI",
                    Publisher = "NXB Trẻ",
                    PublishYear = 2022,
                    ISBN = "8934974180004",
                    Price = 30000,
                    DiscountPrice = 30000,
                    StockQuantity = 60,
                    ImageUrl = "/Content/images/books/jujutsu-4.jpg",
                    Description = "Manga về phép thuật và lời nguyền",
                    Language = "Tiếng Việt",
                    PageCount = 200,
                    CategoryId = 2,
                    IsActive = true
                },
                new Book
                {
                    Title = "Tokyo Ghoul:re Tập 4",
                    Author = "SUI ISHIDA",
                    Publisher = "NXB Trẻ",
                    PublishYear = 2021,
                    ISBN = "8934974170004",
                    Price = 115000,
                    DiscountPrice = 115000,
                    StockQuantity = 35,
                    ImageUrl = "/Content/images/books/tokyo-ghoul-4.jpg",
                    Description = "Thế giới của Ghoul và con người",
                    Language = "Tiếng Việt",
                    PageCount = 220,
                    CategoryId = 2,
                    IsActive = true
                },
                new Book
                {
                    Title = "Cho Tôi Xin Một Vé Đi Tuổi Thơ",
                    Author = "NGUYỄN NHẬT ÁNH",
                    Publisher = "NXB Trẻ",
                    PublishYear = 2018,
                    ISBN = "8934974150000",
                    Price = 80000,
                    DiscountPrice = 60000,
                    StockQuantity = 100,
                    ImageUrl = "/Content/images/books/ve-di-tuoi-tho.jpg",
                    Description = "Hồi ức tuổi thơ đầy cảm xúc",
                    Language = "Tiếng Việt",
                    PageCount = 280,
                    CategoryId = 3,
                    IsActive = true
                },
                new Book
                {
                    Title = "Sapiens",
                    Author = "YUVAL NOAH HARARI",
                    Publisher = "NXB Thế Giới",
                    PublishYear = 2020,
                    ISBN = "8935086850000",
                    Price = 85000,
                    DiscountPrice = 85000,
                    StockQuantity = 45,
                    ImageUrl = "/Content/images/books/sapiens.jpg",
                    Description = "Lược sử loài người",
                    Language = "Tiếng Việt",
                    PageCount = 500,
                    CategoryId = 3,
                    IsActive = true
                }
            };
            books.ForEach(b => context.Books.Add(b));
            context.SaveChanges();

            // Seed Reviews
            var reviews = new List<Review>
            {
                new Review
                {
                    BookId = 1,
                    UserId = 2,
                    Rating = 5,
                    Comment = "Sword Art Online Tập 10 Là Phần Thứ Hai Của Alicization Arc...",
                    ReviewDate = new DateTime(2022, 11, 2)
                },
                new Review
                {
                    BookId = 1,
                    UserId = 3,
                    Rating = 5,
                    Comment = "Sau Khi Tập Đầu Khép Lại Cùng Trận Đấu Với Kirito...",
                    ReviewDate = new DateTime(2022, 11, 2)
                },
                new Review
                {
                    BookId = 1,
                    UserId = 2,
                    Rating = 4,
                    Comment = "Phù, Sau Khi Nghe Giải Thích, Asuna Chọn Đường Gió...",
                    ReviewDate = new DateTime(2022, 8, 6)
                },
                new Review
                {
                    BookId = 1,
                    UserId = 3,
                    Rating = 5,
                    Comment = "Rất Hay! Sau Bài Rà Tập Đầu Tiên Mình Đã Khóp Lại...",
                    ReviewDate = new DateTime(2022, 7, 2)
                }
            };
            reviews.ForEach(r => context.Reviews.Add(r));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}