# Hướng dẫn sửa lỗi Roslyn Compiler

## ✅ Đã sửa lỗi:

### 1. **Xóa Microsoft.CodeDom.Providers.DotNetCompilerPlatform**
- ✅ Xóa reference trong `WebBookStore.csproj`
- ✅ Xóa import trong `WebBookStore.csproj`
- ✅ Xóa package trong `packages.config`
- ✅ Xóa system.codedom trong `Web.config`

### 2. **Clean Build**
- ✅ Xóa thư mục `bin` và `obj`

## 🚀 Cách build project:

### **Phương pháp 1: Visual Studio (Khuyến nghị)**
1. Mở `WebBookStore.sln` trong Visual Studio
2. Nhấn **Ctrl+Shift+B** để build solution
3. Nhấn **F5** để chạy

### **Phương pháp 2: Developer Command Prompt**
1. Mở **Developer Command Prompt for VS** (tìm trong Start Menu)
2. Navigate đến thư mục project:
   ```cmd
   cd "D:\Chuong Trinh Dai Hoc\SV nam 4 2025-2026\web app\DoAn\WebBookStore\WebBookStore"
   ```
3. Build project:
   ```cmd
   msbuild WebBookStore.csproj /p:Configuration=Debug
   ```

### **Phương pháp 3: PowerShell với MSBuild**
1. Mở PowerShell as Administrator
2. Navigate đến thư mục project
3. Tìm MSBuild:
   ```powershell
   Get-ChildItem "C:\Program Files*" -Recurse -Name "MSBuild.exe" | Select-Object -First 1
   ```
4. Build với đường dẫn đầy đủ:
   ```powershell
   & "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" WebBookStore.csproj /p:Configuration=Debug
   ```

## 🔧 Lỗi đã sửa:

### **Lỗi gốc:**
```
Could not find file 'D:\...\WebBookStore\bin\roslyn\csc.exe'
```

### **Nguyên nhân:**
- `Microsoft.CodeDom.Providers.DotNetCompilerPlatform` package bị thiếu hoặc corrupt
- Roslyn compiler không được cài đặt đúng cách

### **Giải pháp:**
- Xóa hoàn toàn Microsoft.CodeDom.Providers.DotNetCompilerPlatform
- Sử dụng compiler mặc định của .NET Framework
- Clean build để xóa cache

## 📋 Kiểm tra sau khi build:

### **Nếu build thành công:**
1. Chạy project (F5)
2. Test đăng ký với dữ liệu:
   - Họ tên: "Nguyễn Hoàng Dũng"
   - Email: "dungdeptrai@gmail.com"
   - Số điện thoại: "0914061758"
   - Mật khẩu: "123456"
   - Nhập lại mật khẩu: "123456"

### **Nếu vẫn có lỗi:**
1. Kiểm tra Visual Studio version (cần 2019 hoặc mới hơn)
2. Cài đặt .NET Framework 4.7.2 Developer Pack
3. Restart Visual Studio
4. Clean và Rebuild solution

## 🎯 Kết quả mong đợi:

- ✅ Project build thành công không có lỗi
- ✅ Website chạy được trên localhost
- ✅ Form đăng ký hoạt động bình thường
- ✅ Debug logs hiển thị trong console và Output window

