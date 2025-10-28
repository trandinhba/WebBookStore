using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using WebBookStore.Data;
using WebBookStore.Models;
using WebBookStore.Services;
using WebBookStore.Repositories;
using WebBookStore.Helpers;
using WebBookStore.Filters;

namespace WebBookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly StoreDbContext _context;
        private readonly IEmailService _emailService;

        public AccountController() : this(new StoreDbContext(), new EmailService())
        {
        }

        public AccountController(StoreDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: Account/Login - Redirect to home since we only use modal login
        public ActionResult Login(string returnUrl)
        {
            // Since we removed the Login view, redirect to home
            return RedirectToAction("Index", "Home");
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password, string returnUrl)
        {
            try
            {
                // Debug: Log all form data
                System.Diagnostics.Debug.WriteLine("=== LOGIN DEBUG ===");
                foreach (string key in Request.Form.AllKeys)
                {
                    System.Diagnostics.Debug.WriteLine($"{key}: {Request.Form[key]}");
                }
                System.Diagnostics.Debug.WriteLine("==================");

                // --- SỬA LỖI ---
                // 1. Lấy giá trị từ form.
                // 2. Cắt bỏ khoảng trắng (Trim) khỏi email/username.
                // 3. KHÔNG CẮT BỎ khoảng trắng khỏi mật khẩu (vì mật khẩu có thể cố ý chứa khoảng trắng).
                var finalEmail = (Request.Form["Email"] ?? email)?.Trim();
                var finalPassword = Request.Form["Password"] ?? password; // Không Trim mật khẩu

                System.Diagnostics.Debug.WriteLine($"Login attempt - Email: {finalEmail}, Password: {finalPassword}");

                // ... (Các dòng Debug kiểm tra hash) ...

                // --- SỬA LỖI ---
                // 1. So sánh email/username KHÔNG PHÂN BIỆT CHỮ HOA/THƯỜNG.
            var user = _context.Users.FirstOrDefault(u =>
                        (u.Username.Equals(finalEmail, StringComparison.OrdinalIgnoreCase) ||
                         u.Email.Equals(finalEmail, StringComparison.OrdinalIgnoreCase)) &&
                u.IsActive);

                System.Diagnostics.Debug.WriteLine($"User found: {user != null}");
                if (user != null)
                {
                    System.Diagnostics.Debug.WriteLine($"User details - Username: {user.Username}, Email: {user.Email}, FullName: {user.FullName}, PasswordHash: {user.PasswordHash}");
                    // --- SỬA LỖI ---
                    // 1. Sử dụng `finalPassword`
                    // 2. Dùng `?? ""` để tránh lỗi Exception nếu `finalPassword` là null.
                    System.Diagnostics.Debug.WriteLine($"Password verification: {VerifyPassword(finalPassword ?? "", user.PasswordHash)}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Login - No user found for email/username: {finalEmail}");
                }

                // --- SỬA LỖI ---
                // 1. Sử dụng `finalPassword`
                // 2. Dùng `?? ""` để tránh lỗi Exception.
                if (user != null && VerifyPassword(finalPassword ?? "", user.PasswordHash))
            {
                Session["UserId"] = user.UserId;
                Session["Username"] = user.Username;
                Session["UserRole"] = user.Role;

                FormsAuthentication.SetAuthCookie(user.Username, false);

                System.Diagnostics.Debug.WriteLine($"[LOGIN SUCCESS] User: {user.Username}, Role: {user.Role}");
                System.Diagnostics.Debug.WriteLine($"[LOGIN SUCCESS] Session UserRole: {Session["UserRole"]}");
                System.Diagnostics.Debug.WriteLine($"[LOGIN SUCCESS] IsAuthenticated: {User.Identity.IsAuthenticated}");
                System.Diagnostics.Debug.WriteLine($"[LOGIN SUCCESS] RoleConstants.ADMIN: {RoleConstants.ADMIN}");
                System.Diagnostics.Debug.WriteLine($"[LOGIN SUCCESS] user.Role == RoleConstants.ADMIN: {user.Role == RoleConstants.ADMIN}");

               // --- SỬA LỖI --- Trả về JSON cho AJAX request
               if (Request.IsAjaxRequest())
               {
                   string redirectUrl = "/";
                   if (user.Role == RoleConstants.ADMIN)
                   {
                       redirectUrl = "/Admin/Dashboard";
                       System.Diagnostics.Debug.WriteLine($"[AJAX LOGIN] Admin detected, redirectUrl: {redirectUrl}");
                       
                       // Force server-side redirect for admin
                       return Json(new { 
                           ok = true, 
                           message = "Đăng nhập thành công!", 
                           redirectUrl = redirectUrl,
                           forceRedirect = true
                       }, JsonRequestBehavior.AllowGet);
                   }
                   else
                   {
                       System.Diagnostics.Debug.WriteLine($"[AJAX LOGIN] Non-admin user, redirectUrl: {redirectUrl}");
                   }
                   
                   System.Diagnostics.Debug.WriteLine($"[AJAX LOGIN] Final JSON response: ok=true, redirectUrl={redirectUrl}");
                   
                   return Json(new { 
                       ok = true, 
                       message = "Đăng nhập thành công!", 
                       redirectUrl = redirectUrl 
                   }, JsonRequestBehavior.AllowGet);
               }

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                if (user.Role == RoleConstants.ADMIN)
                {
                    System.Diagnostics.Debug.WriteLine($"[LOGIN SUCCESS] Redirecting to Admin Dashboard");
                    return RedirectToAction("Dashboard", "Admin");
                }

                System.Diagnostics.Debug.WriteLine($"[LOGIN SUCCESS] Redirecting to Home");
                return RedirectToAction("Index", "Home");
            }

                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng");
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Email = finalEmail; // Truyền email về view để giữ lại giá trị
                if (Request.IsAjaxRequest())
                {
                    return Json(new { ok = false, errors = new[] { "Email hoặc mật khẩu không đúng" } }, JsonRequestBehavior.AllowGet);
                }
                // For non-AJAX requests, redirect to home since we removed the Login view
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Login exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");

            if (Request.IsAjaxRequest())
            {
                    return Json(new { ok = false, errors = new[] { "Có lỗi xảy ra: " + ex.Message } }, JsonRequestBehavior.AllowGet);
                }

                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
            ViewBag.Email = email; // Truyền email về view để giữ lại giá trị
            // For non-AJAX requests, redirect to home since we removed the Login view
            return RedirectToAction("Index", "Home");
            }
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user, string confirmPassword)
        {
            // ... (Các dòng Debug) ...

            // --- SỬA LỖI ---
            // 1. Cắt bỏ khoảng trắng (Trim) khỏi các trường văn bản (FullName, Email, PhoneNumber).
            // 2. KHÔNG CẮT BỎ khoảng trắng khỏi mật khẩu.
            var fullNameFromForm = Request.Form["FullName"]?.Trim();
            var emailFromForm = Request.Form["Email"]?.Trim();
            var passwordFromForm = Request.Form["Password"]; // Không Trim mật khẩu
            var phoneFromForm = Request.Form["PhoneNumber"]?.Trim();
            var confirmPasswordFromForm = Request.Form["ConfirmPassword"]; // Không Trim mật khẩu

            // ... (Các dòng Debug) ...

            // Assign values to user object
            if (!string.IsNullOrWhiteSpace(fullNameFromForm))
            {
                user.FullName = fullNameFromForm;
            }
            if (!string.IsNullOrWhiteSpace(emailFromForm))
            {
                user.Email = emailFromForm;
            }
            if (!string.IsNullOrWhiteSpace(passwordFromForm))
            {
                user.PasswordHash = passwordFromForm;
            }
            if (!string.IsNullOrWhiteSpace(phoneFromForm))
            {
                user.PhoneNumber = phoneFromForm;
            }
            if (!string.IsNullOrWhiteSpace(confirmPasswordFromForm))
            {
                confirmPassword = confirmPasswordFromForm;
            }

            // Generate default username from email if empty
            if (string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.Email))
            {
                user.Username = user.Email.Split('@')[0];
            }

            // ... (Các dòng Debug) ...

            // Clear ModelState to avoid conflicts
            ModelState.Clear();

            // Validation
            if (string.IsNullOrWhiteSpace(user.FullName))
            {
                ModelState.AddModelError("FullName", "Họ tên không được để trống");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                ModelState.AddModelError("Email", "Email không được để trống");
            }
            else if (!IsValidEmail(user.Email))
            {
                ModelState.AddModelError("Email", "Email không hợp lệ");
            }

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                ModelState.AddModelError("Password", "Mật khẩu không được để trống");
            }
            else if (user.PasswordHash.Length < 6)
            {
                ModelState.AddModelError("Password", "Mật khẩu phải có ít nhất 6 ký tự");
            }

            if (user.PasswordHash != confirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Mật khẩu xác nhận không khớp");
            }

            // --- SỬA LỖI ---
            // 1. So sánh KHÔNG PHÂN BIỆT CHỮ HOA/THƯỜNG khi kiểm tra Username/Email tồn tại.
            if (!string.IsNullOrWhiteSpace(user.Username) && _context.Users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Username", "Tên tài khoản đã tồn tại");
            }

            if (!string.IsNullOrWhiteSpace(user.Email) && _context.Users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng");
            }

            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).Where(m => !string.IsNullOrWhiteSpace(m)).ToArray();
                    return Json(new { ok = false, errors = errors.Length > 0 ? errors : new[] { "Dữ liệu không hợp lệ" } }, JsonRequestBehavior.AllowGet);
                }
                return View(user);
            }

            user.PasswordHash = HashPassword(user.PasswordHash);
            user.Role = RoleConstants.CUSTOMER;
            user.IsActive = true;
            user.CreatedDate = DateTime.Now;

            // Debug: Log final user object before saving
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Register - Final user object: Username={user.Username}, Email={user.Email}, FullName={user.FullName}, PasswordHash={user.PasswordHash}");

            _context.Users.Add(user);
            _context.SaveChanges();

            // Debug: Log saved user
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Register - User saved successfully with ID: {user.UserId}");

            // Send welcome email
            _emailService.SendWelcomeEmail(user.Email, user.Username);

            if (Request.IsAjaxRequest())
            {
                return Json(new { ok = true, message = "Đăng ký thành công!" }, JsonRequestBehavior.AllowGet);
            }
            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Logout - Xử lý khi click link đăng xuất
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // POST: Account/Logout - Xử lý khi submit form đăng xuất
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout(string dummy)
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Profile
        [Authorize]
        public new ActionResult Profile()
        {
            var userId = GetCurrentUserId();
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Profile - Current UserId from session: {userId}");

            var user = _context.Users.Find(userId);

            if (user == null)
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Profile - User not found for ID: {userId}");
                return HttpNotFound();
            }

            System.Diagnostics.Debug.WriteLine($"[DEBUG] Profile - User found: ID={user.UserId}, Username={user.Username}, Email={user.Email}, FullName={user.FullName}");
            return View(user);
        }

        // POST: Account/Profile
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public new ActionResult Profile(User model)
        {
            var userId = GetCurrentUserId();
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            // --- SỬA LỖI --- Cắt bỏ khoảng trắng khi cập nhật profile
            user.FullName = model.FullName?.Trim();
            user.Email = model.Email?.Trim();
            user.PhoneNumber = model.PhoneNumber?.Trim();
            user.Address = model.Address?.Trim();

            _context.SaveChanges();

            TempData["Success"] = "Profile updated successfully";
            return RedirectToAction("Profile");
        }

        // GET: Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = _context.Users.Find(userId);

            if (!VerifyPassword(currentPassword, user.PasswordHash))
            {
                ModelState.AddModelError("currentPassword", "Current password is incorrect");
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "Confirm password does not match");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            user.PasswordHash = HashPassword(newPassword);
            _context.SaveChanges();

            TempData["Success"] = "Password changed successfully";
            return RedirectToAction("Profile");
        }

        private string HashPassword(string password)
        {
            // QUAN TRỌNG: Cần xử lý password NULL để tránh Exception
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

        private bool VerifyPassword(string password, string hashedPassword)
        {
            // QUAN TRỌNG: Cần xử lý password NULL để tránh Exception
            if (password == null)
            {
                password = string.Empty;
            }

            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
        }

        private int? GetCurrentUserId()
        {
            if (Session["UserId"] != null && int.TryParse(Session["UserId"].ToString(), out int userId))
            {
                return userId;
            }
            return null;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // ... (Các phương thức External Login và Helper khác không thay đổi) ...

        // External Login Methods - Facebook
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // OAuth URL configuration
            string redirectUri = Url.Action("ExternalLoginCallback", "Account", null, Request.Url.Scheme);
            
            if (provider.ToLower() == "facebook")
            {
                string appId = "YOUR_FACEBOOK_APP_ID"; // TODO: Replace with actual App ID
                string facebookUrl = $"https://www.facebook.com/v13.0/dialog/oauth?client_id={appId}&redirect_uri={redirectUri}&scope=email";
                return Redirect(facebookUrl);
            }
            else if (provider.ToLower() == "google")
            {
                string clientId = "YOUR_GOOGLE_CLIENT_ID"; // TODO: Replace with actual Client ID
                string googleUrl = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope=email profile&access_type=offline";
                return Redirect(googleUrl);
            }
            
            TempData["Message"] = $"Đăng nhập với {provider} đang được phát triển. Vui lòng sử dụng đăng nhập thông thường.";
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string code, string returnUrl)
        {
            try
            {
                // Get user info from OAuth provider
                // TODO: Implement OAuth token exchange and user info retrieval
                
                // For now, create a demo user
                var externalUser = new User
                {
                    Username = "external_user_" + DateTime.Now.Ticks,
                    Email = "external@example.com",
                    FullName = "External User",
                    PasswordHash = HashPassword("external"),
                    Role = RoleConstants.CUSTOMER,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                _context.Users.Add(externalUser);
                _context.SaveChanges();

                Session["UserId"] = externalUser.UserId;
                Session["Username"] = externalUser.Username;
                Session["UserRole"] = externalUser.Role;
                FormsAuthentication.SetAuthCookie(externalUser.Username, false);

                TempData["Message"] = "Đăng nhập thành công!";
                
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                
                if (externalUser.Role == RoleConstants.ADMIN)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Có lỗi xảy ra trong quá trình đăng nhập: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
        
        // Register with Social Providers
        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegisterWithFacebook(string accessToken)
        {
            try
            {
                // TODO: Verify Facebook access token and get user info
                // For demo purposes, create a user
                var user = new User
                {
                    Username = "fb_user_" + DateTime.Now.Ticks,
                    Email = "facebook@example.com",
                    FullName = "Facebook User",
                    PasswordHash = HashPassword("facebook_login"),
                    Role = RoleConstants.CUSTOMER,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };
                
                _context.Users.Add(user);
                _context.SaveChanges();
                
                return Json(new { ok = true, message = "Đăng ký thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegisterWithGoogle(string idToken)
        {
            try
            {
                // TODO: Verify Google ID token and get user info
                // For demo purposes, create a user
                var user = new User
                {
                    Username = "google_user_" + DateTime.Now.Ticks,
                    Email = "google@example.com",
                    FullName = "Google User",
                    PasswordHash = HashPassword("google_login"),
                    Role = RoleConstants.CUSTOMER,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };
                
                _context.Users.Add(user);
                _context.SaveChanges();
                
                return Json(new { ok = true, message = "Đăng ký thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Helper method to reset password for testing
        [AllowAnonymous]
        public ActionResult ResetPassword(string email, string newPassword)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    user.PasswordHash = HashPassword(newPassword);
                    _context.SaveChanges();
                    return Json(new { success = true, message = $"Password đã được reset cho {email}" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "Không tìm thấy user" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Helper method to reset all passwords to fix hash algorithm
        [AllowAnonymous]
        public ActionResult ResetAllPasswords()
        {
            try
            {
                var users = _context.Users.Where(u => u.IsActive).ToList();
                var results = new List<object>();

                foreach (var user in users)
                {
                    var oldHash = user.PasswordHash;
                    user.PasswordHash = HashPassword("123456"); // Reset to default password
                    results.Add(new
                    {
                        email = user.Email,
                        username = user.Username,
                        fullName = user.FullName,
                        oldHash = oldHash,
                        newHash = user.PasswordHash
                    });
                }

                _context.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = $"Đã reset password cho {users.Count} users",
                    users = results
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Test hash method
        [AllowAnonymous]
        public ActionResult TestHash(string password)
        {
            try
            {
                var hash = HashPassword(password ?? "123456");
                var databaseHash = "7e276f74b7507433aa684d27574db0fcf9c1696a9f090b26a5750ce6edfeaa10";
                var matches = hash.Equals(databaseHash, StringComparison.OrdinalIgnoreCase);

                return Json(new
                {
                    success = true,
                    password = password ?? "123456",
                    hash = hash,
                    databaseHash = databaseHash,
                    matches = matches,
                    message = $"Hash của '{password ?? "123456"}' là: {hash}. Database hash: {databaseHash}. Khớp: {matches}"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Reset specific user password
        [AllowAnonymous]
        public ActionResult ResetUserPassword(string email, string newPassword)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    var oldHash = user.PasswordHash;
                    user.PasswordHash = HashPassword(newPassword);
                    _context.SaveChanges();

                    return Json(new
                    {
                        success = true,
                        message = $"Password đã được reset cho {email}",
                        email = user.Email,
                        username = user.Username,
                        fullName = user.FullName,
                        oldHash = oldHash,
                        newHash = user.PasswordHash
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "Không tìm thấy user" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Test multiple passwords to find the correct one
        [AllowAnonymous]
        public ActionResult TestMultiplePasswords()
        {
            try
            {
                var databaseHash = "7e276f74b7507433aa684d27574db0fcf9c1696a9f090b26a5750ce6edfeaa10";
                var testPasswords = new[] { "123456", "123", "password", "admin", "test", "dungdeptrai", "dung", "deptrai", "12345", "111111", "000000", "qwerty", "abc123" };
                var results = new List<object>();

                foreach (var password in testPasswords)
                {
                    var hash = HashPassword(password);
                    var matches = hash.Equals(databaseHash, StringComparison.OrdinalIgnoreCase);
                    results.Add(new
                    {
                        password = password,
                        hash = hash,
                        matches = matches
                    });

                    if (matches)
                    {
                        System.Diagnostics.Debug.WriteLine($"[FOUND] Password '{password}' matches database hash!");
                    }
                }

                return Json(new
                {
                    success = true,
                    databaseHash = databaseHash,
                    results = results,
                    message = "Đã test tất cả passwords phổ biến"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Find correct password for hash
        [AllowAnonymous]
        public ActionResult FindPassword(string hash)
        {
            try
            {
                var testPasswords = new[] { "123456", "123", "password", "admin", "test", "dungdeptrai", "dung", "deptrai", "12345", "111111", "000000", "qwerty", "abc123", "1234", "123456789", "password123" };
                var targetHash = hash ?? "7e276f74b7507433aa684d27574db0fcf9c1696a9f090b26a5750ce6edfeaa10";

                foreach (var password in testPasswords)
                {
                    var computedHash = HashPassword(password);
                    if (computedHash.Equals(targetHash, StringComparison.OrdinalIgnoreCase))
                    {
                        return Json(new
                        {
                            success = true,
                            found = true,
                            password = password,
                            hash = computedHash,
                            message = $"Tìm thấy password: '{password}'"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new
                {
                    success = true,
                    found = false,
                    message = "Không tìm thấy password phù hợp",
                    targetHash = targetHash
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Direct login test with specific credentials
        [AllowAnonymous]
        public ActionResult DirectLoginTest(string email, string password)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[DIRECT LOGIN TEST] Email: {email}, Password: {password}");

                var user = _context.Users.FirstOrDefault(u =>
                    (u.Username.Equals(email, StringComparison.OrdinalIgnoreCase) ||
                     u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)) &&
                    u.IsActive);

                if (user == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "User not found",
                        email = email
                    }, JsonRequestBehavior.AllowGet);
                }

                var passwordHash = HashPassword(password);
                var passwordMatch = VerifyPassword(password, user.PasswordHash);

                System.Diagnostics.Debug.WriteLine($"[DIRECT LOGIN TEST] User: {user.Username}, Email: {user.Email}");
                System.Diagnostics.Debug.WriteLine($"[DIRECT LOGIN TEST] Input password hash: {passwordHash}");
                System.Diagnostics.Debug.WriteLine($"[DIRECT LOGIN TEST] Database hash: {user.PasswordHash}");
                System.Diagnostics.Debug.WriteLine($"[DIRECT LOGIN TEST] Password match: {passwordMatch}");

                return Json(new
                {
                    success = true,
                    userFound = true,
                    passwordMatch = passwordMatch,
                    user = new
                    {
                        username = user.Username,
                        email = user.Email,
                        fullName = user.FullName,
                        databaseHash = user.PasswordHash
                    },
                    inputPasswordHash = passwordHash,
                    message = passwordMatch ? "Login successful!" : "Password does not match"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Debug page for testing login
        [AllowAnonymous]
        public ActionResult LoginDebug()
        {
            return View();
        }

        // Debug page for testing registration issue
        [AllowAnonymous]
        public ActionResult DebugRegistration()
        {
            return View();
        }

        /// <summary>
        /// Kiểm tra và reset Admin account
        /// </summary>
        [AllowAnonymous]
        public ActionResult CheckAndResetAdmin()
        {
            try
            {
                // Kiểm tra Admin account hiện tại
                var adminUser = _context.Users.FirstOrDefault(u => u.Role == RoleConstants.ADMIN);
                
                if (adminUser == null)
                {
                    // Tạo Admin account mới
                    adminUser = new User
                    {
                        Username = "admin",
                        Email = "admin@sach50.com",
                        FullName = "Quản Trị Viên",
                        PasswordHash = HashPassword("admin123"),
                        Role = RoleConstants.ADMIN,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        PhoneNumber = "0123456789",
                        Address = "Hệ thống quản trị"
                    };
                    
                    _context.Users.Add(adminUser);
                    _context.SaveChanges();
                    
                    return Json(new { 
                        success = true, 
                        message = "Admin account đã được tạo mới",
                        adminInfo = new {
                            username = adminUser.Username,
                            email = adminUser.Email,
                            password = "admin123"
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Reset password về mặc định
                    var oldHash = adminUser.PasswordHash;
                    adminUser.PasswordHash = HashPassword("admin123");
                    adminUser.IsActive = true;
                    _context.SaveChanges();
                    
                    return Json(new { 
                        success = true, 
                        message = "Admin account đã được reset",
                        adminInfo = new {
                            username = adminUser.Username,
                            email = adminUser.Email,
                            password = "admin123",
                            oldHash = oldHash,
                            newHash = adminUser.PasswordHash
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = "Có lỗi xảy ra: " + ex.Message 
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Test login với Admin account
        /// </summary>
        [AllowAnonymous]
        public ActionResult TestAdminLogin()
        {
            try
            {
                var adminUser = _context.Users.FirstOrDefault(u => u.Role == RoleConstants.ADMIN);
                
                if (adminUser == null)
                {
                    return Json(new { 
                        success = false, 
                        message = "Không tìm thấy Admin account" 
                    }, JsonRequestBehavior.AllowGet);
                }
                
                var testPassword = "admin123";
                var passwordMatch = VerifyPassword(testPassword, adminUser.PasswordHash);
                
                return Json(new { 
                    success = true, 
                    adminInfo = new {
                        username = adminUser.Username,
                        email = adminUser.Email,
                        fullName = adminUser.FullName,
                        isActive = adminUser.IsActive,
                        passwordHash = adminUser.PasswordHash,
                        testPassword = testPassword,
                        passwordMatch = passwordMatch
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = "Có lỗi xảy ra: " + ex.Message 
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Trang debug Admin account
        /// </summary>
        [AllowAnonymous]
        public ActionResult AdminDebug()
        {
            return View();
        }

        /// <summary>
        /// Debug: Kiểm tra admin user trong database
        /// </summary>
        [AllowAnonymous]
        public ActionResult CheckAdminUser()
        {
            try
            {
                var adminUser = _context.Users.FirstOrDefault(u => u.Role == RoleConstants.ADMIN);
                
                if (adminUser == null)
                {
                    return Json(new { 
                        success = false, 
                        message = "Không tìm thấy Admin user trong database",
                        totalUsers = _context.Users.Count(),
                        allUsers = _context.Users.Select(u => new { u.Username, u.Role, u.IsActive }).ToList()
                    }, JsonRequestBehavior.AllowGet);
                }
                
                // Test password verification with both passwords
                var testPassword1 = "admin123";
                var testPassword2 = "Bao@2004";
                var passwordMatch1 = VerifyPassword(testPassword1, adminUser.PasswordHash);
                var passwordMatch2 = VerifyPassword(testPassword2, adminUser.PasswordHash);
                
                return Json(new { 
                    success = true, 
                    adminInfo = new {
                        userId = adminUser.UserId,
                        username = adminUser.Username,
                        email = adminUser.Email,
                        fullName = adminUser.FullName,
                        role = adminUser.Role,
                        isActive = adminUser.IsActive,
                        passwordHash = adminUser.PasswordHash,
                        testPassword1 = testPassword1,
                        passwordMatch1 = passwordMatch1,
                        testPassword2 = testPassword2,
                        passwordMatch2 = passwordMatch2,
                        createdDate = adminUser.CreatedDate
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = "Có lỗi xảy ra: " + ex.Message,
                    stackTrace = ex.StackTrace
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Reset admin password với hash đúng
        /// </summary>
        [AllowAnonymous]
        public ActionResult ResetAdminPasswordHash()
        {
            try
            {
                var adminUser = _context.Users.FirstOrDefault(u => u.Role == RoleConstants.ADMIN);
                
                if (adminUser == null)
                {
                    return Json(new { 
                        success = false, 
                        message = "Không tìm thấy Admin user" 
                    }, JsonRequestBehavior.AllowGet);
                }
                
                // Reset password với hash đúng
                var newPassword = "admin123";
                var oldHash = adminUser.PasswordHash;
                adminUser.PasswordHash = HashPassword(newPassword);
                adminUser.IsActive = true;
                
                _context.SaveChanges();
                
                // Test verification
                var passwordMatch = VerifyPassword(newPassword, adminUser.PasswordHash);
                
                return Json(new { 
                    success = true, 
                    message = "Admin password đã được reset thành công!",
                    adminInfo = new {
                        username = adminUser.Username,
                        email = adminUser.Email,
                        newPassword = newPassword,
                        oldHash = oldHash,
                        newHash = adminUser.PasswordHash,
                        passwordMatch = passwordMatch,
                        isActive = adminUser.IsActive
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = "Có lỗi xảy ra: " + ex.Message 
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Simple reset admin password
        /// </summary>
        [AllowAnonymous]
        public ActionResult SimpleReset()
        {
            try
            {
                var adminUser = _context.Users.FirstOrDefault(u => u.Role == RoleConstants.ADMIN);
                
                if (adminUser == null)
                {
                    return Content("Không tìm thấy Admin user");
                }
                
                // Reset password
                adminUser.PasswordHash = HashPassword("admin123");
                adminUser.IsActive = true;
                _context.SaveChanges();
                
                return Content("Admin password đã được reset thành công!<br>Username: admin<br>Password: admin123");
            }
            catch (Exception ex)
            {
                return Content("Lỗi: " + ex.Message);
            }
        }

    }
}


