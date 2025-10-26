# Hướng dẫn cấu hình OAuth cho Google và Facebook

## 1. Cấu hình Google OAuth

### Bước 1: Tạo Google OAuth App
1. Truy cập [Google Cloud Console](https://console.cloud.google.com/)
2. Tạo project mới hoặc chọn project hiện có
3. Kích hoạt Google+ API
4. Vào **Credentials** → **Create Credentials** → **OAuth 2.0 Client IDs**
5. Chọn **Web application**
6. Thêm **Authorized redirect URIs**:
   - `https://yourdomain.com/signin-google` (production)
   - `https://localhost:44333/signin-google` (development)

### Bước 2: Cập nhật Web.config
Thay thế các giá trị trong `Web.config`:
```xml
<add key="GoogleClientId" value="YOUR_ACTUAL_GOOGLE_CLIENT_ID" />
<add key="GoogleClientSecret" value="YOUR_ACTUAL_GOOGLE_CLIENT_SECRET" />
```

## 2. Cấu hình Facebook OAuth

### Bước 1: Tạo Facebook App
1. Truy cập [Facebook Developers](https://developers.facebook.com/)
2. Tạo app mới → chọn **Consumer** → **Next**
3. Điền thông tin app
4. Vào **Facebook Login** → **Settings**
5. Thêm **Valid OAuth Redirect URIs**:
   - `https://yourdomain.com/signin-facebook` (production)
   - `https://localhost:44333/signin-facebook` (development)

### Bước 2: Cập nhật Web.config
Thay thế các giá trị trong `Web.config`:
```xml
<add key="FacebookAppId" value="YOUR_ACTUAL_FACEBOOK_APP_ID" />
<add key="FacebookAppSecret" value="YOUR_ACTUAL_FACEBOOK_APP_SECRET" />
```

## 3. Cấu hình SSL (Quan trọng)

OAuth yêu cầu HTTPS. Để test local:
1. Mở project properties
2. Vào **Web** tab
3. Check **Enable SSL**
4. Sử dụng URL HTTPS trong development

## 4. Test

1. Build và chạy project
2. Click "Đăng nhập" hoặc "Đăng ký"
3. Click nút Google/Facebook
4. Kiểm tra xem có redirect đúng không

## 5. Troubleshooting

### Lỗi thường gặp:
- **Redirect URI mismatch**: Kiểm tra URL trong OAuth app settings
- **Invalid client**: Kiểm tra Client ID và Secret
- **SSL required**: Đảm bảo sử dụng HTTPS

### Debug:
- Kiểm tra Console logs
- Xem Network tab trong Developer Tools
- Kiểm tra Application logs

## 6. Production Deployment

Khi deploy lên production:
1. Cập nhật redirect URIs trong OAuth apps
2. Thay đổi Web.config với production URLs
3. Đảm bảo SSL certificate được cài đặt
4. Test lại toàn bộ flow
