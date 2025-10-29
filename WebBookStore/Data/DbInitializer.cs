using System;
using System.Collections.Generic;
using System.Data.Entity;
using WebBookStore.Models;
using System.Security.Cryptography;

namespace WebBookStore.Data
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<StoreDbContext>
    {
        protected override void Seed(StoreDbContext context)
        {
            // Dữ liệu mẫu cho Categories và Users
            var categories = new List<Category>
            {
                new Category { CategoryName = "Light Novel", Description = "Tiểu thuyết nhẹ Nhật Bản" },
                new Category { CategoryName = "Manga", Description = "Truyện tranh Nhật Bản" },
                new Category { CategoryName = "Văn học", Description = "Sách văn học trong và ngoài nước" }
            };
            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();

            // Code cứng tài khoản Admin
            var adminUser = new User 
            { 
                Username = "admin", 
                PasswordHash = HashPassword("admin123"), 
                Email = "admin@sach50.com", 
                FullName = "Quản Trị Viên", 
                Role = "Admin", 
                IsActive = true,
                CreatedDate = DateTime.Now,
                PhoneNumber = "0123456789",
                Address = "Hệ thống quản trị"
            };
            context.Users.Add(adminUser);

            // Thêm test user
            var testUser = new User 
            { 
                Username = "testuser", 
                PasswordHash = HashPassword("123456"), 
                Email = "test@test.com", 
                FullName = "Test User", 
                Role = "Customer", 
                IsActive = true,
                CreatedDate = DateTime.Now
            };
            context.Users.Add(testUser);
            
            context.SaveChanges();

            // Dữ liệu sách đã được sửa lại để dùng đúng các file ảnh tap1.png -> tap8.png
            // và đúng đường dẫn trong thư mục Content
            var books = new List<Book>
            {
                new Book { Title = "Sword Art Online Progressive Tập 1", Author = "REKI KAWAHARA", Publisher = "IPM, Hà Nội", PublishYear = 2022, ISBN = "8935250707640", Price = 120000, StockQuantity = 50, ImageUrl = "~/Content/images/tap1.png", Description = "Hành trình bắt đầu từ tầng 1 của Aincrad. Kirito và Asuna cùng nhau khám phá những bí ẩn của thế giới ảo này.", Language = "Tiếng Việt", PageCount = 320, CategoryId = 1, IsActive = true, CreatedDate = DateTime.Now.AddDays(-10) },
                new Book { Title = "Sword Art Online Progressive Tập 2", Author = "REKI KAWAHARA", Publisher = "IPM, Hà Nội", PublishYear = 2022, ISBN = "8935250707657", Price = 125000, StockQuantity = 40, ImageUrl = "~/Content/images/tap2.png", Description = "Chinh phục tầng thứ hai với những thử thách mới. Cuộc phiêu lưu tiếp tục với những trận chiến gay cấn.", Language = "Tiếng Việt", PageCount = 340, CategoryId = 1, IsActive = true, CreatedDate = DateTime.Now.AddDays(-9) },
                new Book { Title = "Chú Thuật Hồi Chiến Tập 3", Author = "GEGE AKUTAMI", Publisher = "NXB Trẻ", PublishYear = 2023, ISBN = "8935250707664", Price = 35000, StockQuantity = 100, ImageUrl = "~/Content/images/tap5.png", Description = "Những trận chiến chú thuật đỉnh cao. Yuji và các bạn tiếp tục cuộc chiến chống lại các linh hồn.", Language = "Tiếng Việt", PageCount = 200, CategoryId = 2, IsActive = true, CreatedDate = DateTime.Now.AddDays(-8) },
                new Book { Title = "Tokyo Ghoul:re Tập 4", Author = "SUI ISHIDA", Publisher = "NXB Trẻ", PublishYear = 2021, ISBN = "8935250707671", Price = 115000, StockQuantity = 35, ImageUrl = "~/Content/images/tap6.png", Description = "Thế giới của Ghoul và con người đầy phức tạp. Kaneki phải đối mặt với những lựa chọn khó khăn.", Language = "Tiếng Việt", PageCount = 200, CategoryId = 2, IsActive = true, CreatedDate = DateTime.Now.AddDays(-7) },
                new Book { Title = "Cho Tôi Xin Một Vé Đi Tuổi Thơ", Author = "NGUYỄN NHẬT ÁNH", Publisher = "NXB Trẻ", PublishYear = 2018, ISBN = "8935250707688", Price = 80000, DiscountPrice = 60000, StockQuantity = 150, ImageUrl = "~/Content/images/tap7.png", Description = "Hồi ức tuổi thơ trong veo và đầy cảm xúc. Câu chuyện về những kỷ niệm đẹp của tuổi thơ.", Language = "Tiếng Việt", PageCount = 280, CategoryId = 3, IsActive = true, CreatedDate = DateTime.Now.AddDays(-20) },
                new Book { Title = "Sapiens: Lược Sử Loài Người", Author = "YUVAL NOAH HARARI", Publisher = "NXB Thế Giới", PublishYear = 2020, ISBN = "8935250707695", Price = 85000, StockQuantity = 80, ImageUrl = "~/Content/images/tap3.png", Description = "Một cái nhìn toàn cảnh về lịch sử hình thành của nhân loại. Từ thời tiền sử đến hiện tại.", Language = "Tiếng Việt", PageCount = 500, CategoryId = 3, IsActive = true, CreatedDate = DateTime.Now.AddDays(-5) },
                new Book { Title = "Sword Art Online Progressive Tập 7", Author = "REKI KAWAHARA", Publisher = "IPM, Hà Nội", PublishYear = 2023, ISBN = "8935250707701", Price = 130000, StockQuantity = 20, ImageUrl = "~/Content/images/tap4.png", Description = "Những bí ẩn của tầng 7 đang chờ được khám phá. Kirito và Asuna phải đối mặt với thử thách lớn nhất.", Language = "Tiếng Việt", PageCount = 360, CategoryId = 1, IsActive = true, CreatedDate = DateTime.Now.AddDays(-3) },
                new Book { Title = "Chú Thuật Hồi Chiến Tập 8", Author = "GEGE AKUTAMI", Publisher = "NXB Trẻ", PublishYear = 2024, ISBN = "8935250707718", Price = 40000, DiscountPrice = 38000, StockQuantity = 120, ImageUrl = "~/Content/images/tap8.png", Description = "Sự kiện Shibuya bắt đầu leo thang. Cuộc chiến quyết định số phận của thế giới.", Language = "Tiếng Việt", PageCount = 200, CategoryId = 2, IsActive = true, CreatedDate = DateTime.Now.AddDays(-1) }
            };
            books.ForEach(b => context.Books.Add(b));
            context.SaveChanges();

            // Thêm một số đánh giá mẫu
            var sampleReviews = new List<Review>
            {
                new Review { BookId = 1, UserId = 2, Rating = 5, Comment = "Cuốn sách rất hay, nội dung hấp dẫn!", CreatedDate = DateTime.Now.AddDays(-5), ReviewDate = DateTime.Now.AddDays(-5) },
                new Review { BookId = 1, UserId = 2, Rating = 4, Comment = "Tốt nhưng có thể cải thiện thêm.", CreatedDate = DateTime.Now.AddDays(-3), ReviewDate = DateTime.Now.AddDays(-3) },
                new Review { BookId = 7, UserId = 2, Rating = 5, Comment = "Tập 7 thật sự xuất sắc!", CreatedDate = DateTime.Now.AddDays(-2), ReviewDate = DateTime.Now.AddDays(-2) },
                new Review { BookId = 7, UserId = 2, Rating = 5, Comment = "Kirito và Asuna thật tuyệt vời!", CreatedDate = DateTime.Now.AddDays(-1), ReviewDate = DateTime.Now.AddDays(-1) },
                new Review { BookId = 3, UserId = 2, Rating = 4, Comment = "Manga hay, hình ảnh đẹp.", CreatedDate = DateTime.Now.AddDays(-4), ReviewDate = DateTime.Now.AddDays(-4) },
                new Review { BookId = 8, UserId = 2, Rating = 5, Comment = "Tập mới nhất rất hấp dẫn!", CreatedDate = DateTime.Now, ReviewDate = DateTime.Now }
            };
            sampleReviews.ForEach(r => context.Reviews.Add(r));
            context.SaveChanges();

            base.Seed(context);
        }

        private string HashPassword(string password)
        {
            if (password == null)
            {
                password = string.Empty;
            }

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", string.Empty).ToLowerInvariant();
            }
        }
    }
}