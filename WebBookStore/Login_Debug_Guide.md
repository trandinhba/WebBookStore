# Test Password Hashing

## Vấn đề hiện tại:
Tài khoản đã đăng ký thành công vào database nhưng không thể đăng nhập được.

## Nguyên nhân có thể:

### 1. **Password Hashing Algorithm Mismatch**
- Database có password hash: `7e276f74b7507433aa684d27574db0fcf9c1696a9f090b26...`
- Code đang sử dụng SHA256
- Có thể không khớp thuật toán

### 2. **Email Lookup Issue**
- Login tìm user bằng `(u.Username == email || u.Email == email)`
- Database có `Username: dungdeptrai` và `Email: dungdeptrai@gmail.com`
- Có thể lookup không đúng

## Debug Steps:

### **Bước 1: Kiểm tra Password Hash**
1. Mở Visual Studio Output window
2. Thử đăng nhập với:
   - Email: `dungdeptrai@gmail.com`
   - Password: `123456` (password gốc khi đăng ký)
3. Xem debug logs trong Output window

### **Bước 2: Kiểm tra Database**
```sql
SELECT UserId, Username, Email, PasswordHash, FullName, IsActive 
FROM Users 
WHERE Email = 'dungdeptrai@gmail.com' OR Username = 'dungdeptrai'
```

### **Bước 3: Test Hash Algorithm**
```csharp
// Test hash với password "123456"
var testPassword = "123456";
var hashedPassword = HashPassword(testPassword);
Console.WriteLine($"Hashed password: {hashedPassword}");
```

## Expected Debug Output:

### **Login Debug Logs:**
```
=== LOGIN DEBUG ===
Email: dungdeptrai@gmail.com
Password: 123456
==================
Login attempt - Email: dungdeptrai@gmail.com, Password: 123456
User found: True
User details - Username: dungdeptrai, Email: dungdeptrai@gmail.com, PasswordHash: 7e276f74b7507433aa684d27574db0fcf9c1696a9f090b26...
Password verification: True/False
```

## Possible Solutions:

### **Nếu Password Verification = False:**
1. **Reset Password**: Tạo method reset password cho user
2. **Fix Hash Algorithm**: Đảm bảo hash algorithm khớp
3. **Re-register**: Xóa user cũ và đăng ký lại

### **Nếu User Found = False:**
1. **Check Database Connection**: Đảm bảo kết nối database đúng
2. **Check Case Sensitivity**: Kiểm tra case sensitivity
3. **Check IsActive**: Đảm bảo IsActive = 1

## Quick Fix - Reset Password:

Nếu cần reset password cho user hiện tại:

```sql
-- Reset password cho user dungdeptrai
UPDATE Users 
SET PasswordHash = 'e10adc3949ba59abbe56e057f20f883e'  -- hash của "123456"
WHERE Email = 'dungdeptrai@gmail.com'
```

Hoặc xóa user và đăng ký lại:

```sql
-- Xóa user cũ
DELETE FROM Users WHERE Email = 'dungdeptrai@gmail.com'
```

