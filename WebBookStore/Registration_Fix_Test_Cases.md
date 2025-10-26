# Test Cases cho Form Đăng Ký Đã Sửa

## Vấn đề đã sửa:
1. ✅ Thêm field "Họ tên người dùng" 
2. ✅ Sửa lỗi mapping dữ liệu từ form
3. ✅ Cải thiện validation messages
4. ✅ Đổi label từ "Tên tài khoản" thành "Email" cho rõ ràng

## Test Cases:

### Test Case 1: Đăng ký thành công với đầy đủ thông tin
**Input:**
- Họ tên: "Nguyễn Văn A"
- Email: "test@example.com" 
- Số điện thoại: "0912345678"
- Mật khẩu: "123456"
- Nhập lại mật khẩu: "123456"

**Expected:** 
- Đăng ký thành công
- Redirect về trang chủ
- User được tạo trong database với đầy đủ thông tin

### Test Case 2: Đăng ký với email đã tồn tại
**Input:**
- Email đã có trong hệ thống

**Expected:**
- Hiển thị lỗi "Email đã được sử dụng"

### Test Case 3: Đăng ký với mật khẩu không khớp
**Input:**
- Mật khẩu: "123456"
- Nhập lại mật khẩu: "654321"

**Expected:**
- Hiển thị lỗi "Mật khẩu xác nhận không khớp"

### Test Case 4: Đăng ký với thiếu thông tin bắt buộc
**Input:**
- Để trống Họ tên hoặc Email hoặc Mật khẩu

**Expected:**
- Hiển thị lỗi tương ứng:
  - "Họ tên không được để trống"
  - "Email không được để trống" 
  - "Mật khẩu không được để trống"

### Test Case 5: Đăng ký với email không hợp lệ
**Input:**
- Email: "invalid-email"

**Expected:**
- Hiển thị lỗi "Email không hợp lệ"

### Test Case 6: Đăng ký với mật khẩu quá ngắn
**Input:**
- Mật khẩu: "123"

**Expected:**
- Hiển thị lỗi "Mật khẩu phải có ít nhất 6 ký tự"

## Test Đăng Nhập:

### Test Case 7: Đăng nhập thành công
**Input:**
- Email: "test@example.com"
- Mật khẩu: "123456"

**Expected:**
- Đăng nhập thành công
- Hiển thị menu user trong header
- Session được tạo đúng

### Test Case 8: Đăng nhập với thông tin sai
**Input:**
- Email hoặc mật khẩu sai

**Expected:**
- Hiển thị lỗi "Email hoặc mật khẩu không đúng"

## Checklist Test:
- [ ] Form hiển thị đúng các field: Họ tên, Email, Số điện thoại, Mật khẩu, Nhập lại mật khẩu
- [ ] Validation hoạt động đúng cho tất cả field
- [ ] Đăng ký thành công với dữ liệu hợp lệ
- [ ] Error messages hiển thị đúng và đẹp
- [ ] Đăng nhập hoạt động sau khi đăng ký
- [ ] Session và authentication hoạt động đúng
- [ ] UI responsive và user-friendly

