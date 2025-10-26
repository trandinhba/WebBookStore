# 🔧 Debug Admin Login và Dashboard Issues

## 🎯 **Vấn đề cần giải quyết:**

### **1. Đăng nhập từ trang Login không chuyển sang Admin Dashboard**
### **2. Đường dẫn `/Admin` bị lỗi 404**

## 🔍 **Debug Steps:**

### **Step 1: Kiểm tra Login Process**
1. **Mở Console Log** (F12 → Console)
2. **Đăng nhập với Admin:**
   - Username: `admin`
   - Password: `admin123`
3. **Kiểm tra Debug Output** trong Console
4. **Xem có redirect không**

### **Step 2: Kiểm tra Session và Authentication**
1. **Sau khi đăng nhập thành công**
2. **Kiểm tra Session:**
   - `Session["UserId"]` = ?
   - `Session["Username"]` = ?
   - `Session["UserRole"]` = ?
3. **Kiểm tra Authentication:**
   - `User.Identity.IsAuthenticated` = ?
   - `User.Identity.Name` = ?

### **Step 3: Kiểm tra AdminController**
1. **Truy cập trực tiếp:** `/Admin/Dashboard`
2. **Nếu lỗi 404:** Kiểm tra routing
3. **Nếu lỗi 401:** Kiểm tra authorization

## 🔧 **Các Fix đã thực hiện:**

### **1. Thêm Debug Logging:**
```csharp
System.Diagnostics.Debug.WriteLine($"[LOGIN SUCCESS] User: {user.Username}, Role: {user.Role}");
System.Diagnostics.Debug.WriteLine($"[LOGIN SUCCESS] Session UserRole: {Session["UserRole"]}");
System.Diagnostics.Debug.WriteLine($"[LOGIN SUCCESS] IsAuthenticated: {User.Identity.IsAuthenticated}");
```

### **2. Thêm Index Action cho AdminController:**
```csharp
public ActionResult Index()
{
    return RedirectToAction("Dashboard");
}
```

### **3. Cải thiện Redirect Logic:**
```csharp
if (user.Role == RoleConstants.ADMIN)
{
    System.Diagnostics.Debug.WriteLine($"[LOGIN SUCCESS] Redirecting to Admin Dashboard");
    return RedirectToAction("Dashboard", "Admin");
}
```

## 🚀 **Test Cases:**

### **Test 1: Direct Login**
1. Truy cập `/Account/Login`
2. Đăng nhập với `admin` / `admin123`
3. **Expected:** Redirect đến `/Admin/Dashboard`
4. **Actual:** ?

### **Test 2: Direct Admin Access**
1. Truy cập `/Admin`
2. **Expected:** Redirect đến `/Admin/Dashboard`
3. **Actual:** ?

### **Test 3: Direct Dashboard Access**
1. Truy cập `/Admin/Dashboard`
2. **Expected:** Hiển thị Admin Dashboard
3. **Actual:** ?

## 🔍 **Debugging Commands:**

### **1. Kiểm tra Database:**
```sql
SELECT * FROM Users WHERE Username = 'admin' OR Email = 'admin@sach50.com'
```

### **2. Kiểm tra Session:**
- Mở Developer Tools (F12)
- Application → Session Storage
- Kiểm tra các key: UserId, Username, UserRole

### **3. Kiểm tra Cookies:**
- Application → Cookies
- Kiểm tra FormsAuthentication cookie

## 📋 **Possible Issues:**

### **1. Authentication Issue:**
- FormsAuthentication không được set đúng
- Cookie không được tạo
- Session không được lưu

### **2. Authorization Issue:**
- AdminOnly filter không hoạt động
- Role không được check đúng
- Session UserRole bị null

### **3. Routing Issue:**
- Route `/Admin` không được định nghĩa
- Controller không được tìm thấy
- Action không tồn tại

## 🎯 **Next Steps:**

### **1. Test Login:**
- Đăng nhập với Admin account
- Kiểm tra Console logs
- Xem có redirect không

### **2. Test Direct Access:**
- Truy cập `/Admin`
- Truy cập `/Admin/Dashboard`
- Xem lỗi gì

### **3. Check Debug Output:**
- Mở Output window trong Visual Studio
- Xem Debug messages
- Phân tích vấn đề

## 🎉 **Expected Results:**

### **Sau khi fix:**
- ✅ **Login thành công** với Admin account
- ✅ **Redirect đến Dashboard** sau khi login
- ✅ **Truy cập `/Admin`** redirect đến Dashboard
- ✅ **Truy cập `/Admin/Dashboard`** hiển thị giao diện Admin
- ✅ **Session và Authentication** hoạt động đúng

**Hãy test và báo cáo kết quả!** 🚀
