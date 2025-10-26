# 🔧 Sửa Lỗi Đăng Nhập Admin

## 🔍 **Vấn đề:**
Bạn không thể đăng nhập với tài khoản Admin mặc dù đã nhập đúng thông tin.

## ✅ **Giải pháp:**

### **Bước 1: Truy cập trang Debug**
1. Truy cập: `https://localhost:44333/Account/AdminDebug`
2. Trang này sẽ giúp bạn kiểm tra và reset Admin account

### **Bước 2: Kiểm tra Admin Account**
1. Bấm nút **"Kiểm tra Admin Account"**
2. Xem kết quả:
   - ✅ **Admin Account Tồn Tại** - Account đã có
   - ❌ **Không tìm thấy Admin account** - Cần tạo mới

### **Bước 3: Reset Admin Account**
1. Bấm nút **"Reset Admin Account"**
2. Xác nhận reset
3. Password sẽ được đặt lại về `admin123`

### **Bước 4: Thử đăng nhập lại**
1. Truy cập trang đăng nhập
2. Nhập thông tin:
   - **Email/Username:** `admin` hoặc `admin@sach50.com`
   - **Password:** `admin123`

## 🎯 **Thông tin đăng nhập Admin:**

### **Cách 1: Sử dụng Username**
- **Username:** `admin`
- **Password:** `admin123`

### **Cách 2: Sử dụng Email**
- **Email:** `admin@sach50.com`
- **Password:** `admin123`

## 🔧 **Các nguyên nhân có thể:**

### **1. Database chưa được reset**
- Admin account chưa được tạo trong database hiện tại
- Cần reset database hoặc tạo Admin account mới

### **2. Password hash không khớp**
- Password trong database không khớp với "admin123"
- Cần reset password về mặc định

### **3. Account bị vô hiệu hóa**
- `IsActive = false`
- Cần kích hoạt lại account

### **4. Role không đúng**
- Role không phải "Admin"
- Cần sửa role về "Admin"

## 🚀 **Cách sử dụng trang Debug:**

### **Kiểm tra Admin Account:**
- Hiển thị thông tin chi tiết của Admin account
- Kiểm tra password có khớp không
- Kiểm tra account có active không

### **Reset Admin Account:**
- Tạo Admin account mới nếu chưa có
- Reset password về "admin123"
- Kích hoạt account (IsActive = true)
- Hiển thị hash cũ và mới

## 📋 **Kết quả mong đợi:**

### **Sau khi reset thành công:**
```
✅ Admin account đã được reset
Username: admin
Email: admin@sach50.com
Password: admin123
```

### **Sau khi kiểm tra thành công:**
```
✅ Admin Account Tồn Tại
Username: admin
Email: admin@sach50.com
Active: Có
Password Match: Đúng
```

## 🎉 **Sau khi sửa xong:**

1. ✅ **Đăng nhập thành công** với `admin` / `admin123`
2. ✅ **Truy cập Admin Panel** tại `/Admin/Dashboard`
3. ✅ **Giao diện Admin** hiển thị đúng như hình bạn gửi

## 🔒 **Lưu ý bảo mật:**

- ✅ **Chỉ có 1 Admin account** duy nhất
- ✅ **Password mặc định:** `admin123`
- ✅ **Có thể thay đổi password** trong Admin Panel
- ✅ **Account được bảo vệ** bởi `[AdminOnly]` attribute

**Hãy thử trang Debug để sửa lỗi đăng nhập Admin!** 🚀

