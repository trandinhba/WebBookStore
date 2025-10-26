# Debug Instructions cho Form Đăng Ký

## Vấn đề hiện tại:
Form đăng ký vẫn hiển thị lỗi "Tên tài khoản không được để trống" và "Mật khẩu không được để trống" mặc dù đã điền đầy đủ.

## Cách debug:

### Bước 1: Mở Developer Tools
1. Nhấn F12 trong browser
2. Vào tab "Console"
3. Thử đăng ký và xem log

### Bước 2: Kiểm tra Form Data
Trong console sẽ hiển thị:
```
Form data being sent:
FullName: Nguyễn Hoàng Dũng
Email: dungdeptrai@gmail.com
PhoneNumber: 0914061758
Password: [password]
ConfirmPassword: [password]
```

### Bước 3: Kiểm tra Response
Console sẽ hiển thị response từ server:
```
Response: {ok: false, errors: ["Tên tài khoản không được để trống", "Mật khẩu không được để trống"]}
```

## Các nguyên nhân có thể:

### 1. Model Binding Issue
- ASP.NET MVC có thể không bind đúng dữ liệu từ form
- User object có thể null hoặc empty

### 2. Validation Attribute Conflict
- Model có validation attributes conflict với custom validation
- ModelState có thể bị contaminated

### 3. Form Data Encoding
- Dữ liệu có thể bị encode/escape sai
- Special characters có thể gây vấn đề

## Giải pháp đã áp dụng:

### 1. Thêm Debug Logging
- Log tất cả form data trong controller
- Log response trong JavaScript

### 2. Clear ModelState
- `ModelState.Clear()` trước khi validation
- Tránh conflict với model binding

### 3. Sử dụng Request.Form thay vì Request
- `Request.Form["FieldName"]` thay vì `Request["FieldName"]`
- Đảm bảo lấy đúng dữ liệu từ form

### 4. Cải thiện JavaScript
- Log form data trước khi gửi
- Log response để debug
- Clear errors trước khi submit

## Test Steps:

1. **Build và chạy project**
2. **Mở Developer Tools (F12)**
3. **Vào tab Console**
4. **Thử đăng ký với dữ liệu:**
   - Họ tên: "Nguyễn Hoàng Dũng"
   - Email: "dungdeptrai@gmail.com"
   - Số điện thoại: "0914061758"
   - Mật khẩu: "123456"
   - Nhập lại mật khẩu: "123456"
5. **Xem console logs**
6. **Kiểm tra response**

## Expected Results:

### Console Logs:
```
Form data being sent:
FullName: Nguyễn Hoàng Dũng
Email: dungdeptrai@gmail.com
PhoneNumber: 0914061758
Password: 123456
ConfirmPassword: 123456
Response: {ok: true, message: "Đăng ký thành công!"}
```

### Server Debug (Output window):
```
=== REGISTER DEBUG ===
FullName: Nguyễn Hoàng Dũng
Email: dungdeptrai@gmail.com
PhoneNumber: 0914061758
Password: 123456
ConfirmPassword: 123456
=====================
```

Nếu vẫn có lỗi, hãy copy paste console logs để tôi debug tiếp.

