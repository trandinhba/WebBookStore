using System;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Helpers
{
    /// <summary>
    /// Helper class để kiểm tra quyền và thông tin user
    /// </summary>
    public static class PermissionHelper
    {
        /// <summary>
        /// Kiểm tra user hiện tại có role cụ thể không
        /// </summary>
        public static bool HasRole(string requiredRole)
        {
            var currentRole = GetCurrentUserRole();
            return RoleConstants.HasPermission(currentRole, requiredRole);
        }

        /// <summary>
        /// Kiểm tra có phải Admin không
        /// </summary>
        public static bool IsAdmin()
        {
            return HasRole(RoleConstants.ADMIN);
        }

        /// <summary>
        /// Kiểm tra có phải Customer không
        /// </summary>
        public static bool IsCustomer()
        {
            return HasRole(RoleConstants.CUSTOMER);
        }

        /// <summary>
        /// Kiểm tra có phải Guest không
        /// </summary>
        public static bool IsGuest()
        {
            var role = GetCurrentUserRole();
            return RoleConstants.IsGuest(role);
        }

        /// <summary>
        /// Lấy role của user hiện tại
        /// </summary>
        public static string GetCurrentUserRole()
        {
            return HttpContext.Current.Session["UserRole"] as string ?? RoleConstants.GUEST;
        }

        /// <summary>
        /// Lấy UserId của user hiện tại
        /// </summary>
        public static int? GetCurrentUserId()
        {
            return HttpContext.Current.Session["UserId"] as int?;
        }

        /// <summary>
        /// Lấy Username của user hiện tại
        /// </summary>
        public static string GetCurrentUsername()
        {
            return HttpContext.Current.Session["Username"] as string;
        }

        /// <summary>
        /// Kiểm tra user đã đăng nhập chưa
        /// </summary>
        public static bool IsLoggedIn()
        {
            return GetCurrentUserId().HasValue;
        }

        /// <summary>
        /// Lấy thông tin user hiện tại
        /// </summary>
        public static UserInfo GetCurrentUserInfo()
        {
            return new UserInfo
            {
                UserId = GetCurrentUserId(),
                Username = GetCurrentUsername(),
                Role = GetCurrentUserRole(),
                IsLoggedIn = IsLoggedIn()
            };
        }

        /// <summary>
        /// Kiểm tra có thể truy cập chức năng không
        /// </summary>
        public static bool CanAccess(string feature)
        {
            var role = GetCurrentUserRole();
            
            switch (feature.ToLower())
            {
                case "view_products":
                case "browse_catalog":
                    return true; // Guest có thể xem sản phẩm
                
                case "add_to_cart":
                case "place_order":
                case "view_profile":
                case "manage_account":
                    return HasRole(RoleConstants.CUSTOMER);
                
                case "admin_panel":
                case "manage_users":
                case "manage_products":
                case "view_orders":
                case "system_settings":
                    return HasRole(RoleConstants.ADMIN);
                
                default:
                    return HasRole(RoleConstants.CUSTOMER);
            }
        }
    }

    /// <summary>
    /// Class chứa thông tin user hiện tại
    /// </summary>
    public class UserInfo
    {
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public bool IsLoggedIn { get; set; }
        
        public bool IsAdmin => RoleConstants.IsAdmin(Role);
        public bool IsCustomer => RoleConstants.IsCustomer(Role);
        public bool IsGuest => RoleConstants.IsGuest(Role);
    }
}


