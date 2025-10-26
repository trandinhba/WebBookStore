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
            // Kiểm tra user đã đăng nhập chưa
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            // Lấy role từ session
            var userRole = httpContext.Session["UserRole"] as string;
            
            // Kiểm tra quyền
            return RoleConstants.HasPermission(userRole, RequiredRole);
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
