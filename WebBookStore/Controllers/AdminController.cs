using System;
using System.Linq;
using System.Web.Mvc;
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
                // Thống kê tổng quan
                var stats = new
                {
                    TotalUsers = _context.Users.Count(u => u.IsActive),
                    TotalCustomers = _context.Users.Count(u => u.IsActive && u.Role == RoleConstants.CUSTOMER),
                    TotalAdmins = _context.Users.Count(u => u.IsActive && u.Role == RoleConstants.ADMIN),
                    TotalBooks = _context.Books?.Count() ?? 0,
                    TotalOrders = _context.Orders?.Count() ?? 0
                };

                ViewBag.Stats = stats;
                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải dashboard: " + ex.Message;
                return View();
            }
        }

        /// <summary>
        /// Quản lý người dùng
        /// </summary>
        public ActionResult ManageUsers()
        {
            try
            {
                var users = _context.Users
                    .Where(u => u.IsActive)
                    .OrderByDescending(u => u.CreatedDate)
                    .ToList();

                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                return View(users);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra khi tải danh sách người dùng: " + ex.Message;
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
        /// Cài đặt hệ thống
        /// </summary>
        public ActionResult SystemSettings()
        {
            try
            {
                ViewBag.CurrentUser = PermissionHelper.GetCurrentUserInfo();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View();
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