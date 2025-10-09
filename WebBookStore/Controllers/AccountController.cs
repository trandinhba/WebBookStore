using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebBookStore.Data;
using WebBookStore.Models;
using WebBookStore.Services;
using WebBookStore.Repositories;

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

        // GET: Account/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                var emailFromForm = Request["Email"]; // from modal
                if (!string.IsNullOrWhiteSpace(emailFromForm)) username = emailFromForm;
            }
            var user = _context.Users.FirstOrDefault(u =>
                (u.Username == username || u.Email == username) &&
                u.IsActive);

            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                Session["UserId"] = user.UserId;
                Session["Username"] = user.Username;
                Session["UserRole"] = user.Role;

                FormsAuthentication.SetAuthCookie(user.Username, false);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            ViewBag.ReturnUrl = returnUrl;
            if (Request.IsAjaxRequest())
            {
                return Json(new { ok = false, errors = new[] { "Tên đăng nhập hoặc mật khẩu không đúng" } }, JsonRequestBehavior.AllowGet);
            }
            return View();
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
            // Map fields from modal form if needed
            var passwordFromForm = Request["Password"]; // modal/register view
            if (!string.IsNullOrWhiteSpace(passwordFromForm))
            {
                user.PasswordHash = passwordFromForm;
            }
            // Generate default username from email if empty
            if (string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.Email))
            {
                user.Username = user.Email.Split('@')[0];
            }

            if (user.PasswordHash != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "Mật khẩu xác nhận không khớp");
            }

            if (_context.Users.Any(u => u.Username == user.Username))
            {
                ModelState.AddModelError("Username", "Tên tài khoản đã tồn tại");
            }

            if (_context.Users.Any(u => u.Email == user.Email))
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
            user.Role = "Customer";
            user.IsActive = true;
            user.CreatedDate = DateTime.Now;

            _context.Users.Add(user);
            _context.SaveChanges();

            // Send welcome email
            _emailService.SendWelcomeEmail(user.Email, user.Username);

            if (Request.IsAjaxRequest())
            {
                return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
            }
            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        // GET: Account/Logout
        public ActionResult Logout()
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
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

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

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;

            _context.SaveChanges();

            TempData["Success"] = "Cập nhật thông tin thành công";
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
            var user = _context.Users.Find(userId);

            if (!VerifyPassword(currentPassword, user.PasswordHash))
            {
                ModelState.AddModelError("currentPassword", "Mật khẩu hiện tại không đúng");
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "Mật khẩu xác nhận không khớp");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            user.PasswordHash = HashPassword(newPassword);
            _context.SaveChanges();

            TempData["Success"] = "Đổi mật khẩu thành công";
            return RedirectToAction("Profile");
        }

        private string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }

        private int GetCurrentUserId()
        {
            if (Session["UserId"] != null)
            {
                return (int)Session["UserId"];
            }
            return 0;
        }
    }
}