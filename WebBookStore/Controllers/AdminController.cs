using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using WebBookStore.Data;
using WebBookStore.Models;
using WebBookStore.Filters;
using WebBookStore.Helpers;

namespace WebBookStore.Controllers
{
    /// <summary>
    /// Controller dành cho Admin - quản lý hệ thống
    /// </summary>
    [AdminOnly]
    public class AdminController : Controller
    {
        private readonly StoreDbContext _context;

        public AdminController()
        {
            _context = new StoreDbContext();
        }

        /// <summary>
        /// Index action - redirect đến Dashboard
        /// </summary>
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        /// <summary>
        /// Dashboard chính của Admin
        /// </summary>
        public ActionResult Dashboard()
        {
            try
            {
                // Thống kê tổng quan theo giao diện mới
                var stats = new DashboardStats
                {
                    Unresolved = _context.Orders?.Count(o => o.Status == OrderStatus.Pending) ?? 60,
                    Overdue = _context.Orders?.Count(o => o.Status == OrderStatus.Cancelled) ?? 16,
                    InProgress = _context.Orders?.Count(o => o.Status == OrderStatus.Processing) ?? 43,
                    PendingApproval = _context.Orders?.Count(o => o.Status == OrderStatus.Shipped) ?? 64
                };

                // Yêu cầu chưa giải quyết
                var requests = new RequestStats
                {
                    PendingResolution = _context.Orders?.Count(o => o.Status == OrderStatus.Pending) ?? 4238,
                    CustomerBugReports = _context.Orders?.Count(o => o.Status == OrderStatus.Cancelled) ?? 1005,
                    WaitingForDevFix = _context.Orders?.Count(o => o.Status == OrderStatus.Processing) ?? 914,
                    PendingApproval = _context.Orders?.Count(o => o.Status == OrderStatus.Shipped) ?? 281
                };

                // Công việc hôm nay
                var todayTasks = new List<TaskItem>
                {
                    new TaskItem { Id = 1, Text = "Duyệt yêu cầu sửa lỗi", Completed = false, Type = "radio" },
                    new TaskItem { Id = 2, Text = "Fix lỗi", Completed = false, Type = "radio" },
                    new TaskItem { Id = 3, Text = "Giải quyết lỗi giao diện", Completed = true, Type = "checkbox", Tag = "DEFAULT" }
                };

                ViewBag.Stats = stats;
                ViewBag.Requests = requests;
                ViewBag.TodayTasks = todayTasks;
                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                
                return View();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Dashboard exception: {ex.Message}");
                ViewBag.Error = "Có lỗi xảy ra khi tải dashboard: " + ex.Message;
                return View();
            }
        }


        /// <summary>
        /// Xem chi tiết người dùng
        /// </summary>
        public ActionResult UserDetails(int id)
        {
            try
            {
                var user = _context.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound("Không tìm thấy người dùng");
                }

                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View();
            }
        }

        /// <summary>
        /// Vô hiệu hóa người dùng
        /// </summary>
        [HttpPost]
        public ActionResult DeactivateUser(int userId)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng" });
                }

                // Không cho phép vô hiệu hóa Admin
                if (user.Role == RoleConstants.ADMIN)
                {
                    return Json(new { success = false, message = "Không thể vô hiệu hóa tài khoản Admin" });
                }

                user.IsActive = false;
                _context.SaveChanges();

                return Json(new { success = true, message = "Đã vô hiệu hóa người dùng thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// Kích hoạt lại người dùng
        /// </summary>
        [HttpPost]
        public ActionResult ActivateUser(int userId)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng" });
                }

                user.IsActive = true;
                _context.SaveChanges();

                return Json(new { success = true, message = "Đã kích hoạt người dùng thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// Thay đổi role của người dùng
        /// </summary>
        [HttpPost]
        public ActionResult ChangeUserRole(int userId, string newRole)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng" });
                }

                // Không cho phép thay đổi role của Admin
                if (user.Role == RoleConstants.ADMIN)
                {
                    return Json(new { success = false, message = "Không thể thay đổi role của tài khoản Admin" });
                }

                // Chỉ cho phép chuyển thành Customer
                if (newRole != RoleConstants.CUSTOMER)
                {
                    return Json(new { success = false, message = "Chỉ có thể chuyển thành Customer" });
                }

                user.Role = newRole;
                _context.SaveChanges();

                return Json(new { success = true, message = "Đã thay đổi role thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// Quản lý sách
        /// </summary>
        public ActionResult ManageBooks()
        {
            try
            {
                var books = _context.Books
                    .OrderByDescending(b => b.CreatedDate)
                    .ToList();

                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                return View(books);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách sách: " + ex.Message;
                return View();
            }
        }

        /// <summary>
        /// Thêm sách mới
        /// </summary>
        [HttpPost]
        public ActionResult AddBook(string title, string author, string publisher, decimal price, string description, string isbn)
        {
            try
            {
                var book = new Book
                {
                    Title = title,
                    Author = author,
                    Publisher = publisher,
                    Price = price,
                    Description = description,
                    ISBN = isbn,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    StockQuantity = 0, // Default value
                    CategoryId = 1 // Default category
                };

                _context.Books.Add(book);
                _context.SaveChanges();

                return Json(new { success = true, message = "Thêm sách thành công!", bookId = book.BookId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật sách
        /// </summary>
        [HttpPost]
        public ActionResult EditBook(int bookId, string title, string author, string publisher, decimal price, string description, string isbn)
        {
            try
            {
                var book = _context.Books.Find(bookId);
                if (book == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sách" });
                }

                book.Title = title;
                book.Author = author;
                book.Publisher = publisher;
                book.Price = price;
                book.Description = description;
                book.ISBN = isbn;

                _context.SaveChanges();

                return Json(new { success = true, message = "Cập nhật sách thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// Xóa sách
        /// </summary>
        [HttpPost]
        public ActionResult DeleteBook(int bookId)
        {
            try
            {
                var book = _context.Books.Find(bookId);
                if (book == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sách" });
                }

                _context.Books.Remove(book);
                _context.SaveChanges();

                return Json(new { success = true, message = "Xóa sách thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// Quản lý nhân viên - Modified để test
        /// </summary>
        public ActionResult ManageUsers()
        {
            try
            {
                var users = _context.Users
                    .Where(u => u.Role == RoleConstants.ADMIN || u.Role == RoleConstants.EMPLOYEE)
                    .OrderByDescending(u => u.CreatedDate)
                    .ToList();

                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                ViewBag.Title = "Quản Lý Nhân Viên";
                
                return View(users);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách nhân viên: " + ex.Message;
                return View();
            }
        }

        /// <summary>
        /// Quản lý khách hàng
        /// </summary>
        public ActionResult ManageCustomers()
        {
            try
            {
                var customers = _context.Users
                    .Where(u => u.Role == RoleConstants.CUSTOMER)
                    .OrderByDescending(u => u.CreatedDate)
                    .ToList();

                // Nếu không có khách hàng nào, tạo dữ liệu mẫu
                if (!customers.Any())
                {
                    var sampleCustomers = new List<User>
                    {
                        new User
                        {
                            Username = "def345",
                            PasswordHash = "123456",
                            Email = "def345@example.com",
                            FullName = "Trần Thị A",
                            Address = "123 Đường A",
                            PhoneNumber = "0123456789",
                            Role = RoleConstants.CUSTOMER,
                            CreatedDate = DateTime.Now.AddDays(-10),
                            IsActive = true
                        },
                        new User
                        {
                            Username = "ghi789",
                            PasswordHash = "789012",
                            Email = "ghi789@example.com",
                            FullName = "Nguyễn Văn B",
                            Address = "456 Đường B",
                            PhoneNumber = "0987654321",
                            Role = RoleConstants.CUSTOMER,
                            CreatedDate = DateTime.Now.AddDays(-8),
                            IsActive = true
                        },
                        new User
                        {
                            Username = "jkl012",
                            PasswordHash = "345678",
                            Email = "jkl012@example.com",
                            FullName = "Lê Thị C",
                            Address = "789 Đường C",
                            PhoneNumber = "0555666777",
                            Role = RoleConstants.CUSTOMER,
                            CreatedDate = DateTime.Now.AddDays(-6),
                            IsActive = true
                        },
                        new User
                        {
                            Username = "mno345",
                            PasswordHash = "901234",
                            Email = "mno345@example.com",
                            FullName = "Phạm Văn D",
                            Address = "321 Đường D",
                            PhoneNumber = "0333444555",
                            Role = RoleConstants.CUSTOMER,
                            CreatedDate = DateTime.Now.AddDays(-4),
                            IsActive = true
                        },
                        new User
                        {
                            Username = "pqr678",
                            PasswordHash = "567890",
                            Email = "pqr678@example.com",
                            FullName = "Hoàng Thị E",
                            Address = "654 Đường E",
                            PhoneNumber = "0777888999",
                            Role = RoleConstants.CUSTOMER,
                            CreatedDate = DateTime.Now.AddDays(-2),
                            IsActive = true
                        }
                    };

                    _context.Users.AddRange(sampleCustomers);
                    _context.SaveChanges();

                    customers = sampleCustomers;
                }

                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                ViewBag.Title = "Quản Lý Khách Hàng";
                ViewBag.PageTitle = "Quản Lý Khách Hàng";
                return View("ManageUsers", customers); // Reuse the same view
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách khách hàng: " + ex.Message;
                return View("ManageUsers");
            }
        }

        /// <summary>
        /// Simple test action
        /// </summary>
        public ActionResult Test()
        {
            return Json(new { success = true, message = "Test works!" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        [HttpPost]
        public ActionResult TestAction()
        {
            return Json(new { success = true, message = "Test action works!" });
        }

        /// <summary>
        [HttpPost]
        public ActionResult AddEmployee(string username, string password, string fullName, string address, string phoneNumber, string role, string email)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"AddEmployee called: username={username}, role={role}, email={email}");
                
                // Check if username already exists
                if (_context.Users.Any(u => u.Username == username))
                {
                    return Json(new { success = false, message = "Tên đăng nhập đã tồn tại!" });
                }

                var user = new User
                {
                    Username = username,
                    PasswordHash = password, // In production, hash this password
                    Email = email ?? username + "@company.com", // Default email if not provided
                    FullName = fullName,
                    Address = address,
                    PhoneNumber = phoneNumber,
                    Role = role,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                System.Diagnostics.Debug.WriteLine($"AddEmployee success: userId={user.UserId}");
                return Json(new { success = true, message = "Thêm nhân viên thành công!", userId = user.UserId });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AddEmployee error: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        [HttpPost]
        public ActionResult EditEmployee(int userId, string username, string password, string fullName, string address, string phoneNumber, string role, string email)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"EditEmployee called: userId={userId}, username={username}, role={role}");
                
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy nhân viên" });
                }

                // Check if username already exists (excluding current user)
                if (_context.Users.Any(u => u.Username == username && u.UserId != userId))
                {
                    return Json(new { success = false, message = "Tên đăng nhập đã tồn tại!" });
                }

                user.Username = username;
                if (!string.IsNullOrEmpty(password))
                {
                    user.PasswordHash = password; // In production, hash this password
                }
                user.Email = email ?? username + "@company.com";
                user.FullName = fullName;
                user.Address = address;
                user.PhoneNumber = phoneNumber;
                user.Role = role;

                _context.SaveChanges();

                System.Diagnostics.Debug.WriteLine($"EditEmployee success: userId={userId}");
                return Json(new { success = true, message = "Cập nhật nhân viên thành công!" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EditEmployee error: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// Xóa nhân viên
        /// </summary>
        [HttpPost]
        public ActionResult DeleteEmployee(int userId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"DeleteEmployee called: userId={userId}");
                
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy nhân viên" });
                }

                // Prevent deleting admin users
                if (user.Role == RoleConstants.ADMIN)
                {
                    return Json(new { success = false, message = "Không thể xóa tài khoản Admin!" });
                }

                _context.Users.Remove(user);
                _context.SaveChanges();

                System.Diagnostics.Debug.WriteLine($"DeleteEmployee success: userId={userId}");
                return Json(new { success = true, message = "Xóa nhân viên thành công!" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeleteEmployee error: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }


        /// <summary>
        /// Quản lý đơn hàng
        /// </summary>
        public ActionResult ManageOrders()
        {
            try
            {
                var orders = _context.Orders
                    .OrderByDescending(o => o.CreatedAt)
                    .ToList();

                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                ViewBag.Title = "Quản Lý Đơn Hàng";
                return View(orders);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách đơn hàng: " + ex.Message;
                return View();
            }
        }

        /// <summary>
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        [HttpPost]
        public ActionResult UpdateOrderStatus(int orderId, string status)
        {
            try
            {
                var order = _context.Orders.Find(orderId);
                if (order == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
                }

                if (Enum.TryParse<OrderStatus>(status, true, out OrderStatus newStatus))
                {
                    order.Status = newStatus;
                    order.UpdatedAt = DateTime.Now;
                    _context.SaveChanges();

                    return Json(new { success = true, message = "Cập nhật trạng thái đơn hàng thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Trạng thái không hợp lệ" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// Xem chi tiết đơn hàng
        /// </summary>
        public ActionResult OrderDetails(int id)
        {
            try
            {
                var order = _context.Orders
                    .FirstOrDefault(o => o.Id == id);

                if (order == null)
                {
                    return HttpNotFound("Không tìm thấy đơn hàng");
                }

                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                return View(order);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View();
            }
        }

        /// <summary>
        /// Quản lý kho
        /// </summary>
        public ActionResult ManageInventory()
        {
            try
            {
                var books = _context.Books.ToList();
                
                // Sort manually to avoid LINQ expression issues
                books = books.OrderBy(b => b.StockQuantity).ToList();
                books = books.OrderByDescending(b => b.CreatedDate).ToList();

                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                ViewBag.Title = "Quản Lý Kho";
                return View("ManageInventory", books);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách kho: " + ex.Message;
                return View();
            }
        }

        /// <summary>
        /// Cập nhật số lượng tồn kho
        /// </summary>
        [HttpPost]
        public ActionResult UpdateStock(int bookId, int quantity)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"UpdateStock called: bookId={bookId}, quantity={quantity}");
                
                var book = _context.Books.Find(bookId);
                if (book == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sách" });
                }

                book.StockQuantity = quantity;
                _context.SaveChanges();

                System.Diagnostics.Debug.WriteLine($"UpdateStock success: bookId={bookId}");
                return Json(new { success = true, message = "Cập nhật số lượng tồn kho thành công!" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateStock error: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật trạng thái sách (Active/Inactive)
        /// </summary>
        [HttpPost]
        public ActionResult UpdateBookStatus(int bookId, bool isActive)
        {
            try
            {
                var book = _context.Books.Find(bookId);
                if (book == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sách" });
                }

                book.IsActive = isActive;
                _context.SaveChanges();

                return Json(new { success = true, message = "Cập nhật trạng thái sách thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// System Settings
        /// </summary>
        public ActionResult SystemSettings()
        {
            try
            {
                var stats = new
                {
                    TotalBooks = _context.Books.Count(),
                    TotalUsers = _context.Users.Count(),
                    TotalOrders = _context.Orders.Count(),
                    TotalCategories = _context.Categories.Count(),
                    LowStockBooks = _context.Books.Count(b => b.StockQuantity < 10),
                    PendingOrders = _context.Orders.Count(o => o.Status == OrderStatus.Pending)
                };

                ViewBag.Stats = stats;
                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                ViewBag.Title = "Cài Đặt Hệ Thống";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View();
            }
        }

        /// <summary>
        /// Upload ảnh cho sách
        /// </summary>
        [HttpPost]
        public ActionResult UploadBookImage()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("UploadBookImage called");
                
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    System.Diagnostics.Debug.WriteLine($"File received: {file?.FileName}, Size: {file?.ContentLength}");
                    
                    if (file != null && file.ContentLength > 0)
                    {
                        // Validate file type
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
                        
                        System.Diagnostics.Debug.WriteLine($"File extension: {fileExtension}");
                        
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            return Json(new { success = false, message = "Chỉ chấp nhận file: JPG, JPEG, PNG, GIF" });
                        }

                        // Validate file size (max 5MB)
                        if (file.ContentLength > 5 * 1024 * 1024)
                        {
                            return Json(new { success = false, message = "File không được vượt quá 5MB" });
                        }

                        // Get bookId from request
                        int bookId = 0;
                        if (!int.TryParse(Request.Form["bookId"], out bookId))
                        {
                            System.Diagnostics.Debug.WriteLine("Could not parse bookId from form");
                            return Json(new { success = false, message = "Không tìm thấy ID sách" });
                        }

                        System.Diagnostics.Debug.WriteLine($"BookId: {bookId}");

                        // Create directory if not exists
                        var uploadPath = "/Content/uploads/books/";
                        var physicalPath = Server.MapPath(uploadPath);
                        
                        System.Diagnostics.Debug.WriteLine($"Physical path: {physicalPath}");
                        
                        if (!System.IO.Directory.Exists(physicalPath))
                        {
                            System.IO.Directory.CreateDirectory(physicalPath);
                            System.Diagnostics.Debug.WriteLine("Created directory");
                        }

                        // Generate unique filename
                        var fileName = $"book_{bookId}_{DateTime.Now.Ticks}{fileExtension}";
                        var filePath = System.IO.Path.Combine(physicalPath, fileName);
                        
                        System.Diagnostics.Debug.WriteLine($"Saving to: {filePath}");
                        
                        // Save file
                        file.SaveAs(filePath);

                        // Update book with image path
                        var book = _context.Books.Find(bookId);
                        if (book != null)
                        {
                            // Delete old image if exists
                            if (!string.IsNullOrEmpty(book.ImageUrl) && System.IO.File.Exists(Server.MapPath(book.ImageUrl)))
                            {
                                System.IO.File.Delete(Server.MapPath(book.ImageUrl));
                            }

                            book.ImageUrl = uploadPath + fileName;
                            _context.SaveChanges();

                            System.Diagnostics.Debug.WriteLine($"Image saved successfully: {book.ImageUrl}");
                            
                            return Json(new { 
                                success = true, 
                                message = "Upload ảnh thành công!", 
                                imageUrl = book.ImageUrl 
                            });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Không tìm thấy sách với ID: " + bookId });
                        }
                    }
                }

                return Json(new { success = false, message = "Không có file được chọn" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Upload error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// Xóa ảnh của sách
        /// </summary>
        [HttpPost]
        public ActionResult DeleteBookImage(int bookId)
        {
            try
            {
                var book = _context.Books.Find(bookId);
                if (book == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sách" });
                }

                if (!string.IsNullOrEmpty(book.ImageUrl))
                {
                    var filePath = Server.MapPath(book.ImageUrl);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                book.ImageUrl = null;
                _context.SaveChanges();

                return Json(new { success = true, message = "Xóa ảnh thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// Reset Admin password
        /// </summary>
        [HttpPost]
        public ActionResult ResetAdminPassword()
        {
            try
            {
                var adminUser = _context.Users.FirstOrDefault(u => u.Role == RoleConstants.ADMIN);
                if (adminUser == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy Admin account" });
                }

                // Hash password mới
                var newPassword = "admin123"; // Có thể randomize
                adminUser.PasswordHash = HashPassword(newPassword);
                _context.SaveChanges();

                return Json(new { 
                    success = true, 
                    message = "Admin password đã được reset",
                    newPassword = newPassword
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        /// <summary>
        /// Helper method để hash password
        /// </summary>
        private string HashPassword(string password)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}