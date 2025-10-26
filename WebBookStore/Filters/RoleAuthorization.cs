using System;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Filters
{
    /// <summary>
    /// Attribute để kiểm tra quyền truy cập theo role
    /// </summary>
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public string RequiredRole { get; set; }
        public string RedirectAction { get; set; } = "Login";
        public string RedirectController { get; set; } = "Account";

        public RoleAuthorizeAttribute(string requiredRole = RoleConstants.CUSTOMER)
        {
            RequiredRole = requiredRole;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Debug logging
            System.Diagnostics.Debug.WriteLine($"[AUTHORIZE] Checking authorization for role: {RequiredRole}");
            System.Diagnostics.Debug.WriteLine($"[AUTHORIZE] IsAuthenticated: {httpContext.User.Identity.IsAuthenticated}");
            System.Diagnostics.Debug.WriteLine($"[AUTHORIZE] User.Identity.Name: {httpContext.User.Identity.Name}");
            
            // Lấy role từ session TRƯỚC
            var userRole = httpContext.Session["UserRole"] as string;
            System.Diagnostics.Debug.WriteLine($"[AUTHORIZE] Session UserRole: {userRole}");
            
            // Kiểm tra có session role không (ưu tiên session hơn FormsAuthentication)
            if (!string.IsNullOrEmpty(userRole))
            {
                System.Diagnostics.Debug.WriteLine($"[AUTHORIZE] Using session role: {userRole}");
                var hasPermission = RoleConstants.HasPermission(userRole, RequiredRole);
                System.Diagnostics.Debug.WriteLine($"[AUTHORIZE] HasPermission: {hasPermission}");
                return hasPermission;
            }
            
            // Fallback: Kiểm tra user đã đăng nhập chưa
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                System.Diagnostics.Debug.WriteLine($"[AUTHORIZE] User not authenticated and no session role, denying access");
                return false;
            }

            // Nếu có FormsAuthentication nhưng không có session role
            System.Diagnostics.Debug.WriteLine($"[AUTHORIZE] Has FormsAuthentication but no session role, denying access");
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                // Trả về JSON cho AJAX request
                var redirectUrl = new UrlHelper(filterContext.RequestContext).Action(RedirectAction, RedirectController);
                filterContext.Result = new JsonResult
                {
                    Data = new { 
                        ok = false, 
                        message = "Bạn không có quyền truy cập chức năng này",
                        redirect = redirectUrl
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                // Redirect cho normal request
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = RedirectController, action = RedirectAction })
                );
            }
        }
    }

    /// <summary>
    /// Attribute chỉ dành cho Admin
    /// </summary>
    public class AdminOnlyAttribute : RoleAuthorizeAttribute
    {
        public AdminOnlyAttribute() : base(RoleConstants.ADMIN)
        {
        }
    }

    /// <summary>
    /// Attribute dành cho Customer trở lên
    /// </summary>
    public class CustomerOnlyAttribute : RoleAuthorizeAttribute
    {
        public CustomerOnlyAttribute() : base(RoleConstants.CUSTOMER)
        {
        }
    }

    /// <summary>
    /// Attribute cho phép Guest truy cập (không cần đăng nhập)
    /// </summary>
    public class AllowGuestAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // Luôn cho phép truy cập
            return;
        }
    }
}
