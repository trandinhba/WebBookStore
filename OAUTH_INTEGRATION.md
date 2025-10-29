# Hướng dẫn tích hợp Google & Facebook Login

## Facebook OAuth

### 1. Tạo Facebook App
1. Truy cập: https://developers.facebook.com/
2. Tạo App mới > Chọn "Consumer" > Đặt tên app
3. Vào Settings > Basic để lấy App ID và App Secret
4. Thêm Facebook Login product
5. Cấu hình Valid OAuth Redirect URIs: `https://yourdomain.com/Account/ExternalLoginCallback`

### 2. Cập nhật code
Thay thế `YOUR_FACEBOOK_APP_ID` trong `AdminController.cs`:
```csharp
string appId = "YOUR_ACTUAL_FACEBOOK_APP_ID";
```

### 3. Cài đặt Facebook SDK
Thêm vào Register.cshtml:
```html
<script async defer src="https://connect.facebook.net/en_US/sdk.js"></script>
<script>
  window.fbAsyncInit = function() {
    FB.init({
      appId: 'YOUR_FACEBOOK_APP_ID',
      cookie: true,
      xfbml: true,
      version: 'v13.0'
    });
  };
</script>
```

## Google OAuth

### 1. Tạo Google Project
1. Truy cập: https://console.cloud.google.com/
2. Tạo project mới
3. Vào APIs & Services > Credentials
4. Create Credentials > OAuth client ID
5. Application type: Web application
6. Authorized redirect URIs: `https://yourdomain.com/Account/ExternalLoginCallback`
7. Lấy Client ID và Client Secret

### 2. Cập nhật code
Thay thế `YOUR_GOOGLE_CLIENT_ID` trong `AdminController.cs`:
```csharp
string clientId = "YOUR_ACTUAL_GOOGLE_CLIENT_ID";
```

### 3. Cài đặt Google Sign-In
Thêm vào Register.cshtml:
```html
<meta name="google-signin-client_id" content="YOUR_GOOGLE_CLIENT_ID">
<script src="https://apis.google.com/js/platform.js" async defer></script>
```

## Security Notes

⚠️ **Quan trọng:**
- Không commit App ID/Secret vào Git
- Sử dụng Configuration Manager để quản lý credentials
- Enable HTTPS trong production
- Validate tokens server-side

## Testing

1. **Local Development**: Sử dụng `http://localhost:port` trong OAuth config
2. **Production**: Update redirect URIs với domain thực tế
3. **Test Flow**: Click nút Social login > Complete OAuth flow > Verify session

## Production Checklist

- [ ] Đã cập nhật App ID/Secret
- [ ] Đã thêm redirect URIs
- [ ] Đã enable HTTPS
- [ ] Đã test OAuth flow
- [ ] Đã cấu hình session management
- [ ] Đã setup error handling

