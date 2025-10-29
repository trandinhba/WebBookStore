using System;
using System.Linq;
using WebBookStore.Data;
using WebBookStore.Models;

namespace WebBookStore
{
    /// <summary>
    /// Script để sửa lỗi đăng nhập customer và test các chức năng
    /// </summary>
    public class FixCustomerLogin
    {
        public static void Main()
        {
            Console.WriteLine("=== FIX CUSTOMER LOGIN ===");
            
            using (var context = new StoreDbContext())
            {
                // 1. Kiểm tra test user có tồn tại không
                var testUser = context.Users.FirstOrDefault(u => u.Username == "testuser");
                
                if (testUser == null)
                {
                    Console.WriteLine("❌ Test user không tồn tại. Tạo mới...");
                    
                    // Tạo test user mới
                    testUser = new User
                    {
                        Username = "testuser",
                        PasswordHash = HashPassword("123456"),
                        Email = "test@test.com",
                        FullName = "Test Customer",
                        Role = RoleConstants.CUSTOMER,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        PhoneNumber = "0123456789",
                        Address = "123 Test Street"
                    };
                    
                    context.Users.Add(testUser);
                    context.SaveChanges();
                    Console.WriteLine("✅ Đã tạo test user mới");
                }
                else
                {
                    Console.WriteLine("✅ Test user đã tồn tại");
                }
                
                // 2. Test password hashing
                Console.WriteLine("\n=== TEST PASSWORD HASHING ===");
                var testPassword = "123456";
                var hashedPassword = HashPassword(testPassword);
                var passwordMatch = VerifyPassword(testPassword, testUser.PasswordHash);
                
                Console.WriteLine($"Test password: {testPassword}");
                Console.WriteLine($"Hashed password: {hashedPassword}");
                Console.WriteLine($"Database hash: {testUser.PasswordHash}");
                Console.WriteLine($"Password match: {passwordMatch}");
                
                if (!passwordMatch)
                {
                    Console.WriteLine("❌ Password không khớp. Reset password...");
                    testUser.PasswordHash = hashedPassword;
                    context.SaveChanges();
                    Console.WriteLine("✅ Đã reset password");
                }
                
                // 3. Kiểm tra role
                Console.WriteLine($"\nUser role: {testUser.Role}");
                Console.WriteLine($"Is Active: {testUser.IsActive}");
                
                // 4. Test login logic
                Console.WriteLine("\n=== TEST LOGIN LOGIC ===");
                var loginTest = TestLogin("testuser", "123456", context);
                Console.WriteLine($"Login test result: {loginTest}");
                
                // 5. Tạo thêm customer test
                Console.WriteLine("\n=== CREATE ADDITIONAL TEST CUSTOMERS ===");
                CreateTestCustomers(context);
                
                Console.WriteLine("\n=== SUMMARY ===");
                Console.WriteLine("Test accounts created:");
                Console.WriteLine("1. Username: testuser, Password: 123456, Email: test@test.com");
                Console.WriteLine("2. Username: customer1, Password: 123456, Email: customer1@test.com");
                Console.WriteLine("3. Username: customer2, Password: 123456, Email: customer2@test.com");
                Console.WriteLine("\nAdmin account:");
                Console.WriteLine("Username: admin, Password: admin123, Email: admin@sach50.com");
            }
        }
        
        private static bool TestLogin(string username, string password, StoreDbContext context)
        {
            var user = context.Users.FirstOrDefault(u =>
                (u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) ||
                 u.Email.Equals(username, StringComparison.OrdinalIgnoreCase)) &&
                u.IsActive);
                
            if (user == null)
            {
                Console.WriteLine("❌ User not found");
                return false;
            }
            
            var passwordMatch = VerifyPassword(password, user.PasswordHash);
            Console.WriteLine($"User found: {user.Username}, Role: {user.Role}");
            Console.WriteLine($"Password match: {passwordMatch}");
            
            return passwordMatch;
        }
        
        private static void CreateTestCustomers(StoreDbContext context)
        {
            var customers = new[]
            {
                new User
                {
                    Username = "customer1",
                    PasswordHash = HashPassword("123456"),
                    Email = "customer1@test.com",
                    FullName = "Customer One",
                    Role = RoleConstants.CUSTOMER,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    PhoneNumber = "0987654321",
                    Address = "456 Customer Street"
                },
                new User
                {
                    Username = "customer2",
                    PasswordHash = HashPassword("123456"),
                    Email = "customer2@test.com",
                    FullName = "Customer Two",
                    Role = RoleConstants.CUSTOMER,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    PhoneNumber = "0555666777",
                    Address = "789 Customer Avenue"
                }
            };
            
            foreach (var customer in customers)
            {
                if (!context.Users.Any(u => u.Username == customer.Username))
                {
                    context.Users.Add(customer);
                    Console.WriteLine($"✅ Created customer: {customer.Username}");
                }
                else
                {
                    Console.WriteLine($"⚠️ Customer already exists: {customer.Username}");
                }
            }
            
            context.SaveChanges();
        }
        
        private static string HashPassword(string password)
        {
            if (password == null)
            {
                password = string.Empty;
            }

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", string.Empty).ToLowerInvariant();
            }
        }
        
        private static bool VerifyPassword(string password, string hashedPassword)
        {
            if (password == null)
            {
                password = string.Empty;
            }

            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }
}

