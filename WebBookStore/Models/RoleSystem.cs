using System;

namespace WebBookStore.Models
{
    /// <summary>
    /// Enum định nghĩa các role trong hệ thống
    /// </summary>
    public enum UserRole
    {
        Guest = 0,      // Khách - chỉ xem sản phẩm
        Customer = 1,   // Khách hàng - có thể mua hàng
        Admin = 2       // Quản trị viên - toàn quyền
    }

    /// <summary>
    /// Constants cho hệ thống phân quyền
    /// </summary>
    public static class RoleConstants
    {
        // Role names
        public const string GUEST = "Guest";
        public const string CUSTOMER = "Customer";
        public const string ADMIN = "Admin";

        // Admin account (hardcoded)
        public const string ADMIN_USERNAME = "admin";
        public const string ADMIN_EMAIL = "admin@sach50.com";
        public const string ADMIN_PASSWORD = "admin123";
        public const string ADMIN_FULLNAME = "Quản Trị Viên";

        // Permission levels
        public const int GUEST_LEVEL = 0;
        public const int CUSTOMER_LEVEL = 1;
        public const int ADMIN_LEVEL = 2;

        /// <summary>
        /// Kiểm tra role có quyền cao hơn hoặc bằng role khác không
        /// </summary>
        public static bool HasPermission(string userRole, string requiredRole)
        {
            var userLevel = GetRoleLevel(userRole);
            var requiredLevel = GetRoleLevel(requiredRole);
            return userLevel >= requiredLevel;
        }

        /// <summary>
        /// Lấy level của role
        /// </summary>
        public static int GetRoleLevel(string role)
        {
            switch (role?.ToLower())
            {
                case "admin": return ADMIN_LEVEL;
                case "customer": return CUSTOMER_LEVEL;
                case "guest":
                default: return GUEST_LEVEL;
            }
        }

        /// <summary>
        /// Kiểm tra có phải Admin không
        /// </summary>
        public static bool IsAdmin(string role)
        {
            return string.Equals(role, ADMIN, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Kiểm tra có phải Customer không
        /// </summary>
        public static bool IsCustomer(string role)
        {
            return string.Equals(role, CUSTOMER, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Kiểm tra có phải Guest không
        /// </summary>
        public static bool IsGuest(string role)
        {
            return string.IsNullOrEmpty(role) || string.Equals(role, GUEST, StringComparison.OrdinalIgnoreCase);
        }
    }
}

