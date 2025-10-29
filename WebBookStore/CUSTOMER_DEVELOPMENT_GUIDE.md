# 🚀 HƯỚNG DẪN PHÁT TRIỂN CHỨC NĂNG CUSTOMER

## 📋 **THỨ TỰ PHÁT TRIỂN**

### **1. KHẮC PHỤC ĐĂNG NHẬP CUSTOMER (ƯU TIÊN CAO NHẤT)**

#### **Bước 1: Test đăng nhập**
1. Chạy project và truy cập: `https://localhost:44333/Account/TestLogin`
2. Test với các tài khoản:
   - **Customer**: `testuser` / `123456`
   - **Admin**: `admin` / `admin123`

#### **Bước 2: Tạo thêm test customers**
Truy cập: `https://localhost:44333/Account/CreateTestCustomers`

#### **Bước 3: Test modal login**
1. Vào trang chủ: `https://localhost:44333/`
2. Click "Tài Khoản" → "Đăng nhập"
3. Test đăng nhập với modal

---

### **2. PHÁT TRIỂN CHỨC NĂNG GIỎ HÀNG**

#### **Sau khi customer đăng nhập được:**

**A. Thêm sản phẩm vào giỏ hàng**
- Vào trang sách: `https://localhost:44333/Books`
- Click "Thêm Giỏ Hàng" trên sản phẩm
- Kiểm tra số lượng trong header

**B. Xem giỏ hàng**
- Click icon giỏ hàng trong header
- URL: `https://localhost:44333/Cart`

**C. Cập nhật giỏ hàng**
- Thay đổi số lượng
- Xóa sản phẩm

---

### **3. PHÁT TRIỂN CHỨC NĂNG ĐẶT HÀNG**

**A. Checkout**
- Từ giỏ hàng, click "Thanh toán"
- URL: `https://localhost:44333/Order/Checkout`

**B. Tạo đơn hàng**
- Điền thông tin giao hàng
- Xác nhận đơn hàng

**C. Xem lịch sử đơn hàng**
- URL: `https://localhost:44333/Order/History`

---

### **4. PHÁT TRIỂN CHỨC NĂNG ĐÁNH GIÁ SÁCH**

**A. Thêm đánh giá**
- Vào trang chi tiết sách
- Scroll xuống phần đánh giá
- Thêm rating và comment

**B. Hiển thị đánh giá**
- Xem đánh giá của người khác
- Tính điểm trung bình

---

### **5. PHÁT TRIỂN DANH SÁCH YÊU THÍCH**

**A. Thêm vào wishlist**
- Click icon tim trên sản phẩm
- Hoặc từ trang chi tiết sách

**B. Xem wishlist**
- URL: `https://localhost:44333/Wishlist`

---

## 🔧 **CÁC URL QUAN TRỌNG**

### **Customer URLs:**
- Trang chủ: `/`
- Danh sách sách: `/Books`
- Chi tiết sách: `/Books/Details/{id}`
- Giỏ hàng: `/Cart`
- Checkout: `/Order/Checkout`
- Lịch sử đơn hàng: `/Order/History`
- Wishlist: `/Wishlist`
- Profile: `/Account/Profile`

### **Admin URLs:**
- Dashboard: `/Admin/Dashboard`
- Quản lý sách: `/Admin/ManageBooks`
- Quản lý người dùng: `/Admin/ManageUsers`
- Quản lý đơn hàng: `/Admin/ManageOrders`

### **Test URLs:**
- Test login: `/Account/TestLogin`
- Reset admin: `/Account/SimpleReset`
- Create test users: `/Account/CreateTestCustomers`

---

## 🐛 **CÁCH DEBUG VÀ SỬA LỖI**

### **1. Kiểm tra Session**
```javascript
// Trong browser console
console.log('Session UserId:', '@Session["UserId"]');
console.log('Session Username:', '@Session["Username"]');
console.log('Session UserRole:', '@Session["UserRole"]');
```

### **2. Kiểm tra Database**
- Mở SQL Server Management Studio
- Connect đến database `WebBookStoreDb`
- Kiểm tra bảng `Users`, `Books`, `Orders`

### **3. Debug Login**
- Mở Visual Studio Debug
- Set breakpoint trong `AccountController.Login()`
- Test đăng nhập và xem giá trị

---

## 📝 **CHECKLIST PHÁT TRIỂN**

### **Phase 1: Authentication ✅**
- [ ] Test customer login
- [ ] Test admin login
- [ ] Modal login/register
- [ ] Session management

### **Phase 2: Shopping Cart 🔄**
- [ ] Add to cart
- [ ] View cart
- [ ] Update quantities
- [ ] Remove items
- [ ] Cart count in header

### **Phase 3: Order Management 📋**
- [ ] Checkout process
- [ ] Create order
- [ ] Order confirmation
- [ ] Order history
- [ ] Order details

### **Phase 4: Reviews & Wishlist ⭐**
- [ ] Add reviews
- [ ] Display reviews
- [ ] Add to wishlist
- [ ] View wishlist
- [ ] Remove from wishlist

---

## 🚨 **LƯU Ý QUAN TRỌNG**

1. **Luôn test với customer account trước**
2. **Kiểm tra session sau mỗi action**
3. **Test trên nhiều browser khác nhau**
4. **Kiểm tra database sau mỗi thao tác**
5. **Backup database trước khi test**

---

## 🎯 **MỤC TIÊU CUỐI CÙNG**

Customer có thể:
1. ✅ Đăng nhập/đăng ký
2. ✅ Xem danh sách sách
3. ✅ Thêm sách vào giỏ hàng
4. ✅ Xem và quản lý giỏ hàng
5. ✅ Đặt hàng và thanh toán
6. ✅ Xem lịch sử đơn hàng
7. ✅ Đánh giá sách
8. ✅ Quản lý danh sách yêu thích
9. ✅ Cập nhật thông tin cá nhân

**Bắt đầu với việc test đăng nhập customer ngay bây giờ!** 🚀

