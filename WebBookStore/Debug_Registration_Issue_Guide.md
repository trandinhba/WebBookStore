# Debug Registration Issue - "trần đình bá" hiển thị "dungdeptrai"

## 🔍 **Vấn đề:**

1. **Đăng ký với "trần đình bá"** → Profile hiển thị "dungdeptrai"
2. **Sau khi đăng xuất** → Không thể đăng nhập lại với tài khoản "trần đình bá"

## 🚀 **Cách Debug:**

### **Bước 1: Build và chạy project**
1. Build project trong Visual Studio (Ctrl+Shift+B)
2. Chạy project (F5)

### **Bước 2: Truy cập trang debug**
1. Mở browser và vào: `https://localhost:44333/Account/DebugRegistration`
2. Thực hiện các test theo thứ tự

### **Bước 3: Test Sequence**

#### **Test 1: Register New User**
1. Điền form với:
   - Họ tên: "trần đình bá"
   - Email: "trandinba@gmail.com"
   - Số điện thoại: "0912345678"
   - Mật khẩu: "123456"
   - Nhập lại mật khẩu: "123456"
2. Click "Đăng ký"
3. Xem kết quả

#### **Test 2: Login Test**
1. Điền form với:
   - Email: "trandinba@gmail.com"
   - Mật khẩu: "123456"
2. Click "Đăng nhập"
3. Xem kết quả

#### **Test 3: Check Profile**
1. Click "Kiểm tra Profile"
2. Xem profile hiển thị gì

#### **Test 4: Database Check**
1. Click "Kiểm tra Database"
2. Xem debug logs

### **Bước 4: Kiểm tra Debug Logs**

#### **Mở Visual Studio Output Window:**
1. View → Output
2. Chọn "Debug" từ dropdown
3. Xem logs khi thực hiện các test

#### **Expected Debug Logs:**

**Register Debug:**
```
=== REGISTER DEBUG ===
FullName: trần đình bá
Email: trandinba@gmail.com
Password: 123456
PhoneNumber: 0912345678
ConfirmPassword: 123456
=====================
[DEBUG] Register - FullName from form: trần đình bá
[DEBUG] Register - Email from form: trandinba@gmail.com
[DEBUG] Register - User object FullName: trần đình bá
[DEBUG] Register - User object Username: trandinba
[DEBUG] Register - Final user object: Username=trandinba, Email=trandinba@gmail.com, FullName=trần đình bá
[DEBUG] Register - User saved successfully with ID: [ID]
```

**Login Debug:**
```
[DEBUG] Login - All active users in database:
[DEBUG] Login - User: ID=1, Username=dungdeptrai, Email=dungdeptrai@gmail.com, FullName=Nguyễn Hoàng Dũng
[DEBUG] Login - User: ID=2, Username=trandinba, Email=trandinba@gmail.com, FullName=trần đình bá
[DEBUG] Login - User found: True
[DEBUG] Login - User details - Username: trandinba, Email: trandinba@gmail.com, FullName: trần đình bá
[DEBUG] Login - Password verification: True/False
```

**Profile Debug:**
```
[DEBUG] Profile - Current UserId from session: [ID]
[DEBUG] Profile - User found: ID=[ID], Username=trandinba, Email=trandinba@gmail.com, FullName=trần đình bá
```

## 🔧 **Possible Issues:**

### **Issue 1: Session Problem**
- Session["UserId"] có thể bị lưu sai
- Profile method lấy sai user ID

### **Issue 2: Database Problem**
- User được lưu với FullName sai
- Username bị overwrite

### **Issue 3: Login Lookup Problem**
- Login tìm user sai
- Password verification fail

## 📋 **Debug Steps:**

### **Nếu Register thành công nhưng Profile sai:**
1. Kiểm tra Session["UserId"] trong Profile method
2. Kiểm tra database có user đúng không
3. Kiểm tra GetCurrentUserId() method

### **Nếu Login thất bại:**
1. Kiểm tra user lookup trong database
2. Kiểm tra password verification
3. Kiểm tra email/username matching

### **Nếu Database có user sai:**
1. Kiểm tra Register method mapping
2. Kiểm tra form data binding
3. Kiểm tra validation logic

## 🎯 **Expected Results:**

- ✅ Register với "trần đình bá" thành công
- ✅ Profile hiển thị "trần đình bá"
- ✅ Login với "trandinba@gmail.com" thành công
- ✅ Debug logs hiển thị đúng thông tin

**Hãy thực hiện các test và gửi cho tôi debug logs!** 🔍

