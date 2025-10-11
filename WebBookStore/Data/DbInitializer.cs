using System;
using System.Collections.Generic;
using System.Data.Entity;
using WebBookStore.Models;

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

            var users = new List<User> { new User { Username = "testuser", PasswordHash = "123", Email = "test@test.com", FullName = "Test User", Role = "Customer", IsActive = true } };
            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            // Dữ liệu sách đã được sửa lại để dùng đúng các file ảnh tap1.png -> tap8.png
            // và đúng đường dẫn trong thư mục Content
            var books = new List<Book>
            {
                new Book { Title = "Sword Art Online Progressive Tập 1", Author = "REKI KAWAHARA", Publisher = "IPM, Hà Nội", PublishYear = 2022, Price = 120000, StockQuantity = 50, ImageUrl = "~/Content/images/tap1.png", Description = "Hành trình bắt đầu từ tầng 1 của Aincrad.", Language = "Tiếng Việt", CategoryId = 1, IsActive = true, CreatedDate = DateTime.Now.AddDays(-10) },
                new Book { Title = "Sword Art Online Progressive Tập 2", Author = "REKI KAWAHARA", Publisher = "IPM, Hà Nội", PublishYear = 2022, Price = 125000, StockQuantity = 40, ImageUrl = "~/Content/images/tap2.png", Description = "Chinh phục tầng thứ hai với những thử thách mới.", Language = "Tiếng Việt", CategoryId = 1, IsActive = true, CreatedDate = DateTime.Now.AddDays(-9) },
                new Book { Title = "Chú Thuật Hồi Chiến Tập 3", Author = "GEGE AKUTAMI", Publisher = "NXB Trẻ", PublishYear = 2023, Price = 35000, StockQuantity = 100, ImageUrl = "~/Content/images/tap3.png", Description = "Những trận chiến chú thuật đỉnh cao.", Language = "Tiếng Việt", CategoryId = 2, IsActive = true, CreatedDate = DateTime.Now.AddDays(-8) },
                new Book { Title = "Tokyo Ghoul:re Tập 4", Author = "SUI ISHIDA", Publisher = "NXB Trẻ", PublishYear = 2021, Price = 115000, StockQuantity = 35, ImageUrl = "~/Content/images/tap4.png", Description = "Thế giới của Ghoul và con người đầy phức tạp.", Language = "Tiếng Việt", CategoryId = 2, IsActive = true, CreatedDate = DateTime.Now.AddDays(-7) },
                new Book { Title = "Cho Tôi Xin Một Vé Đi Tuổi Thơ", Author = "NGUYỄN NHẬT ÁNH", Publisher = "NXB Trẻ", PublishYear = 2018, Price = 80000, DiscountPrice = 60000, StockQuantity = 150, ImageUrl = "~/Content/images/tap5.png", Description = "Hồi ức tuổi thơ trong veo và đầy cảm xúc.", Language = "Tiếng Việt", CategoryId = 3, IsActive = true, CreatedDate = DateTime.Now.AddDays(-20) },
                new Book { Title = "Sapiens: Lược Sử Loài Người", Author = "YUVAL NOAH HARARI", Publisher = "NXB Thế Giới", PublishYear = 2020, Price = 85000, StockQuantity = 80, ImageUrl = "~/Content/images/tap6.png", Description = "Một cái nhìn toàn cảnh về lịch sử hình thành của nhân loại.", Language = "Tiếng Việt", CategoryId = 3, IsActive = true, CreatedDate = DateTime.Now.AddDays(-5) },
                new Book { Title = "Sword Art Online Progressive Tập 7", Author = "REKI KAWAHARA", Publisher = "IPM, Hà Nội", PublishYear = 2023, Price = 130000, StockQuantity = 20, ImageUrl = "~/Content/images/tap7.png", Description = "Những bí ẩn của tầng 7 đang chờ được khám phá.", Language = "Tiếng Việt", CategoryId = 1, IsActive = true, CreatedDate = DateTime.Now.AddDays(-3) },
                new Book { Title = "Chú Thuật Hồi Chiến Tập 8", Author = "GEGE AKUTAMI", Publisher = "NXB Trẻ", PublishYear = 2024, Price = 40000, DiscountPrice = 38000, StockQuantity = 120, ImageUrl = "~/Content/images/tap8.png", Description = "Sự kiện Shibuya bắt đầu leo thang.", Language = "Tiếng Việt", CategoryId = 2, IsActive = true, CreatedDate = DateTime.Now.AddDays(-1) }
            };
            books.ForEach(b => context.Books.Add(b));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}