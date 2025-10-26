# Test Cases cho Authentication System

## 1. Test Đăng ký thông thường

### Test Case 1.1: Đăng ký thành công
- **Input**: Email hợp lệ, mật khẩu >= 6 ký tự, confirm password khớp
- **Expected**: Tạo user mới, redirect về trang chủ
- **Steps**:
  1. Click "Đăng ký"
  2. Nhập email: test@example.com
  3. Nhập password: 123456
  4. Nhập confirm password: 123456
  5. Click "Đăng ký"
  6. Verify: Thông báo thành công, user được tạo trong DB

### Test Case 1.2: Đăng ký với email đã tồn tại
- **Input**: Email đã có trong hệ thống
- **Expected**: Hiển thị lỗi "Email đã được sử dụng"
- **Steps**:
  1. Thử đăng ký với email đã tồn tại
  2. Verify: Hiển thị lỗi validation

### Test Case 1.3: Đăng ký với mật khẩu không khớp
- **Input**: Password và confirm password khác nhau
- **Expected**: Hiển thị lỗi "Mật khẩu xác nhận không khớp"

## 2. Test Đăng nhập thông thường

### Test Case 2.1: Đăng nhập thành công
- **Input**: Email và password đúng
- **Expected**: Đăng nhập thành công, hiển thị menu user
- **Steps**:
  1. Click "Đăng nhập"
  2. Nhập email: test@example.com
  3. Nhập password: 123456
  4. Click "Đăng nhập"
  5. Verify: Header hiển thị tên user, có dropdown menu

### Test Case 2.2: Đăng nhập với thông tin sai
- **Input**: Email hoặc password sai
- **Expected**: Hiển thị lỗi "Incorrect username or password"

## 3. Test Social Login

### Test Case 3.1: Đăng nhập Google thành công
- **Prerequisites**: Đã cấu hình Google OAuth
- **Steps**:
  1. Click "Đăng nhập với Google"
  2. Redirect đến Google OAuth
  3. Chọn tài khoản Google
  4. Authorize app
  5. Verify: Redirect về app, đăng nhập thành công

### Test Case 3.2: Đăng nhập Facebook thành công
- **Prerequisites**: Đã cấu hình Facebook OAuth
- **Steps**:
  1. Click "Đăng nhập với Facebook"
  2. Redirect đến Facebook OAuth
  3. Chọn tài khoản Facebook
  4. Authorize app
  5. Verify: Redirect về app, đăng nhập thành công

### Test Case 3.3: Tạo user mới từ Social Login
- **Steps**:
  1. Đăng nhập Google/Facebook với email chưa có trong hệ thống
  2. Verify: User mới được tạo tự động
  3. Verify: User được đăng nhập ngay

## 4. Test UI/UX

### Test Case 4.1: Modal hoạt động đúng
- **Steps**:
  1. Click "Đăng nhập" → Modal hiển thị
  2. Click "Đăng ký" → Modal đăng ký hiển thị
  3. Click "Đăng ký ngay" từ modal đăng nhập → Chuyển sang modal đăng ký
  4. Click outside modal → Modal đóng

### Test Case 4.2: Loading states
- **Steps**:
  1. Submit form đăng nhập/đăng ký
  2. Verify: Button hiển thị spinner và text "Đang xử lý..."
  3. Verify: Button bị disable trong lúc xử lý

### Test Case 4.3: Error handling
- **Steps**:
  1. Submit form với dữ liệu không hợp lệ
  2. Verify: Hiển thị lỗi với styling đẹp
  3. Verify: Form không bị reset

## 5. Test Session Management

### Test Case 5.1: Session persistence
- **Steps**:
  1. Đăng nhập thành công
  2. Refresh trang
  3. Verify: Vẫn đăng nhập

### Test Case 5.2: Logout
- **Steps**:
  1. Đăng nhập thành công
  2. Click dropdown user menu
  3. Click "Đăng xuất"
  4. Verify: Session bị clear, hiển thị menu đăng nhập

## 6. Test Cart Integration

### Test Case 6.1: Cart count hiển thị đúng
- **Steps**:
  1. Đăng nhập
  2. Thêm sản phẩm vào giỏ hàng
  3. Verify: Số lượng giỏ hàng cập nhật trong header

## Checklist Test

- [ ] Đăng ký thành công
- [ ] Đăng ký với email trùng
- [ ] Đăng ký với password không khớp
- [ ] Đăng nhập thành công
- [ ] Đăng nhập với thông tin sai
- [ ] Google OAuth (nếu đã cấu hình)
- [ ] Facebook OAuth (nếu đã cấu hình)
- [ ] Modal UI hoạt động đúng
- [ ] Loading states
- [ ] Error messages
- [ ] Session persistence
- [ ] Logout
- [ ] Cart count integration
