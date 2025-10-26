# Hướng dẫn implement OAuth sau này

## Vấn đề hiện tại
Do lỗi `HttpContextBase does not contain a definition for 'GetOwinContext'`, tôi đã tạm thời disable OAuth và thay thế bằng thông báo "đang phát triển".

## Cách sửa lỗi OAuth

### Bước 1: Kiểm tra packages
Đảm bảo các package sau đã được cài đặt:
```xml
<package id="Microsoft.Owin.Host.SystemWeb" version="4.2.2" />
<package id="Microsoft.Owin.Security.Google" version="4.2.2" />
<package id="Microsoft.Owin.Security.Facebook" version="4.2.2" />
```

### Bước 2: Thêm using statements
Trong `AccountController.cs`, thêm:
```csharp
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;
```

### Bước 3: Sửa AuthenticationManager
Thay thế method `AuthenticationManager`:
```csharp
private IAuthenticationManager AuthenticationManager
{
    get
    {
        return HttpContext.GetOwinContext().Authentication;
    }
}
```

### Bước 4: Restore External Login Methods
Thay thế các method simplified bằng code OAuth đầy đủ:

```csharp
[HttpPost]
[AllowAnonymous]
[ValidateAntiForgeryToken]
public ActionResult ExternalLogin(string provider, string returnUrl)
{
    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
}

[AllowAnonymous]
public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
{
    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
    if (loginInfo == null)
    {
        return RedirectToAction("Login");
    }

    var email = loginInfo.Email;
    var user = _context.Users.FirstOrDefault(u => u.Email == email);

    if (user == null)
    {
        user = new User
        {
            Email = email,
            Username = email.Split('@')[0],
            FullName = loginInfo.DefaultUserName ?? email.Split('@')[0],
            PasswordHash = HashPassword(Guid.NewGuid().ToString()),
            Role = "Customer",
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        _context.Users.Add(user);
        _context.SaveChanges();
    }

    Session["UserId"] = user.UserId;
    Session["Username"] = user.Username;
    Session["UserRole"] = user.Role;

    FormsAuthentication.SetAuthCookie(user.Username, false);

    return RedirectToLocal(returnUrl);
}

internal class ChallengeResult : HttpUnauthorizedResult
{
    public ChallengeResult(string provider, string redirectUri)
    {
        LoginProvider = provider;
        RedirectUri = redirectUri;
    }

    public string LoginProvider { get; set; }
    public string RedirectUri { get; set; }

    public override void ExecuteResult(ControllerContext context)
    {
        var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
    }
}
```

### Bước 5: Cập nhật UI
Thay thế các button "coming soon" bằng form submit thực tế:

```html
@using (Html.BeginForm("ExternalLogin", "Account", new { ReturnUrl = Request.Url.AbsolutePath }, FormMethod.Post))
{
    @Html.AntiForgeryToken()
    <input type="hidden" name="provider" value="Google" />
    <button type="submit" class="btn btn-outline-danger w-100">
        <i class="bi bi-google me-2"></i>Đăng nhập với Google
    </button>
}
```

## Test OAuth
1. Cấu hình OAuth credentials trong Web.config
2. Test với HTTPS (localhost:44333)
3. Kiểm tra redirect URIs trong OAuth apps

## Lưu ý
- OAuth yêu cầu HTTPS
- Cần cấu hình redirect URIs đúng
- Test trên localhost trước khi deploy production

