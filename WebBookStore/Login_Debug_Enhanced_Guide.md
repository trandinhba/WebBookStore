# 🔍 Debug Login Issue - Password 123456 không đúng

## 🚨 **Vấn đề hiện tại:**

1. **Password "123456" không đúng** - Login vẫn fail
2. **Lỗi "Có lỗi xảy ra"** - Có exception trong login process
3. **Password hash không khớp** - Database vẫn có hash cũ

## 🔧 **Giải pháp đã thêm:**

### **1. Thêm Exception Handling:**
- ✅ Wrap login method trong try-catch
- ✅ Log exception details trong debug
- ✅ Return error message thay vì crash

### **2. Thêm Test Methods:**
- ✅ `TestHash()` - Test hash algorithm
- ✅ `ResetUserPassword()` - Reset password cho 1 user cụ thể
- ✅ Enhanced debug logging

### **3. Cập nhật Debug Page:**
- ✅ Test Hash của "123456"
- ✅ Reset User Password cho dungdeptrai@gmail.com
- ✅ Chi tiết hash comparison

## 🚀 **Cách Debug:**

### **Bước 1: Build và chạy project**
1. Build project trong Visual Studio (Ctrl+Shift+B)
2. Chạy project (F5)

### **Bước 2: Truy cập trang debug**
1. Mở browser và vào: `https://localhost:44333/Account/LoginDebug`
2. Thực hiện các test theo thứ tự

### **Bước 3: Test Sequence**

#### **Test 1: Test Hash**
1. Click **"Test Hash của '123456'"**
2. Xem hash được tạo ra
3. Ghi nhớ hash này để so sánh

#### **Test 2: Reset User Password**
1. Click **"Reset Password cho dungdeptrai@gmail.com"**
2. Xem old hash vs new hash
3. Kiểm tra new hash có khớp với Test 1 không

#### **Test 3: Test Login**
1. Click **"Test Login"** với:
   - Email: `dungdeptrai@gmail.com`
   - Password: `123456`
2. Xem kết quả

### **Bước 4: Kiểm tra Debug Logs**

#### **Mở Visual Studio Output Window:**
1. View → Output
2. Chọn "Debug" từ dropdown
3. Xem logs khi thực hiện các test

#### **Expected Debug Logs:**

**Test Hash:**
```
Hash của '123456' là: e10adc3949ba59abbe56e057f20f883e
```

**Reset User Password:**
```json
{
  "success": true,
  "message": "Password đã được reset cho dungdeptrai@gmail.com",
  "email": "dungdeptrai@gmail.com",
  "username": "dungdeptrai",
  "fullName": "Nguyễn Hoàng Dũng",
  "oldHash": "7e276f74b7507433aa684d27574db0fcf9c1696a9f090b26...",
  "newHash": "e10adc3949ba59abbe56e057f20f883e"
}
```

**Login Debug:**
```
=== LOGIN DEBUG ===
Email: dungdeptrai@gmail.com
Password: 123456
==================
Login attempt - Email: dungdeptrai@gmail.com, Password: 123456
[DEBUG] Login - All active users in database:
[DEBUG] Login - User: ID=2, Username=dungdeptrai, Email=dungdeptrai@gmail.com, FullName=Nguyễn Hoàng Dũng
User found: True
User details - Username: dungdeptrai, Email: dungdeptrai@gmail.com, FullName: Nguyễn Hoàng Dũng, PasswordHash: e10adc3949ba59abbe56e057f20f883e
Password verification: True
```

## 🔍 **Possible Issues:**

### **Issue 1: Hash Algorithm Mismatch**
- Test Hash tạo hash khác với database
- **Giải pháp**: Reset password để tạo hash mới

### **Issue 2: Database Not Updated**
- Reset password không thành công
- **Giải pháp**: Kiểm tra database connection

### **Issue 3: Exception in Login**
- Có lỗi trong login process
- **Giải pháp**: Xem exception details trong debug logs

## 📋 **Debug Steps:**

### **Nếu Test Hash thành công:**
1. Ghi nhớ hash được tạo
2. Reset User Password
3. Kiểm tra new hash có khớp không

### **Nếu Reset User Password thành công:**
1. Kiểm tra new hash có khớp với Test Hash không
2. Test Login với password "123456"
3. Xem debug logs

### **Nếu Login vẫn fail:**
1. Kiểm tra exception trong debug logs
2. Kiểm tra user lookup
3. Kiểm tra password verification

## 🎯 **Expected Results:**

- ✅ Test Hash tạo hash: `e10adc3949ba59abbe56e057f20f883e`
- ✅ Reset User Password thành công với new hash khớp
- ✅ Login với "123456" thành công
- ✅ Debug logs hiển thị "Password verification: True"
- ✅ User được redirect về trang chủ

## ⚠️ **Lưu ý:**

- **Hash Algorithm**: SHA256 với BitConverter
- **Password**: Tất cả users sẽ có password "123456"
- **Debug**: Xem Output window để debug chi tiết

**Hãy thực hiện các test và gửi cho tôi kết quả!** 🔍

