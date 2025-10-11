# WebBookStore - Website Bán Sách Trực Tuyến

Đây là một dự án website bán sách trực tuyến được xây dựng trên nền tảng ASP.NET MVC 5 và Entity Framework 6.

## Mục Lục

1.  [Công nghệ sử dụng](https://www.google.com/search?q=%23c%C3%B4ng-ngh%E1%BB%87-s%E1%BB%AD-d%E1%BB%A5ng)
2.  [Các chức năng chính](https://www.google.com/search?q=%23c%C3%A1c-ch%E1%BB%A9c-n%C4%83ng-ch%C3%ADnh)
3.  [Hướng dẫn cài đặt và khởi chạy](https://www.google.com/search?q=%23h%C6%B0%E1%BB%9Bng-d%E1%BA%ABn-c%C3%A0i-%C4%91%E1%BA%B7t-v%C3%A0-kh%E1%BB%9Fi-ch%E1%BA%A1y)
4.  [Tích hợp giao diện](https://www.google.com/search?q=%23t%C3%ADch-h%E1%BB%A3p-giao-di%E1%BB%87n)

## Công nghệ sử dụng

  * **Backend:** ASP.NET MVC 5, Entity Framework 6
  * **Database:** SQL Server
  * **Frontend:** HTML, CSS, JavaScript, jQuery, Bootstrap 5
  * **Kiến trúc:** Code-First, Repository Pattern, Service Layer

## Các chức năng chính

  * Hiển thị danh sách sản phẩm (Sản phẩm bán chạy, Sách mới nhất).
  * Xem chi tiết thông tin sản phẩm.
  * Chức năng giỏ hàng (Thêm, Xóa, Cập nhật số lượng).
  * Đặt hàng và thanh toán (Mô phỏng).
  * Đánh giá sản phẩm (Bình luận và chấm điểm từ 1-5 sao).
  * Thêm sản phẩm vào danh sách yêu thích (Wishlist).
  * Tìm kiếm sản phẩm.
  * Quản lý người dùng (Đăng ký, Đăng nhập).

## Hướng dẫn cài đặt và khởi chạy

Để khởi chạy dự án này trên máy cục bộ, vui lòng thực hiện theo các bước sau.

#### Yêu cầu môi trường

  * **Visual Studio:** 2019 hoặc 2022 (Đã cài đặt workload "ASP.NET and web development").
  * **SQL Server:** Bất kỳ phiên bản nào (ví dụ: Express, Developer, Standard).

-----

#### Bước 1: Mở Dự Án

  * Mở file `WebBookStore.sln` bằng Visual Studio.

-----

#### Bước 2: Cấu hình Connection String

Project sử dụng hai database riêng biệt. Bạn cần chỉnh sửa chuỗi kết nối để trỏ đến máy chủ SQL Server của mình.

1.  Mở file `Web.config` ở thư mục gốc của dự án.

2.  Tìm đến phần `<connectionStrings>`.

3.  Sửa `Data Source=KENNYNGUYEN` thành tên máy chủ SQL Server của bạn.

    ```xml
    <connectionStrings>
      <add name="DefaultConnection"
           connectionString="Data Source=TEN_SERVER_SQL_CUA_BAN;Initial Catalog=WebBookStore_Users;Integrated Security=True;MultipleActiveResultSets=True"
           providerName="System.Data.SqlClient" />

      <add name="StoreDbContext"
           connectionString="Data Source=TEN_SERVER_SQL_CUA_BAN;Initial Catalog=WebBookStoreDb;Integrated Security=True;MultipleActiveResultSets=True"
           providerName="System.Data.SqlClient" />
    </connectionStrings>
    ```

-----

#### Bước 3: Cấu hình Khởi Tạo Database

Để đảm bảo database được tự động tạo và có dữ liệu mẫu khi chạy lần đầu, hãy chắc chắn rằng file `Global.asax.cs` đã được cấu hình đúng.

1.  Mở file `Global.asax.cs`.

2.  Trong hàm `Application_Start()`, đảm bảo có dòng sau:

    ```csharp
    Database.SetInitializer(new DbInitializer());
    ```

-----

#### Bước 4: Khôi Phục Gói NuGet và Sửa Lỗi Môi Trường

1.  Trong cửa sổ **Solution Explorer**, nhấp chuột phải vào Solution (`WebBookStore`) và chọn **Restore NuGet Packages**.
2.  Nếu gặp lỗi liên quan đến `roslyn\csc.exe` khi chạy, hãy mở **Package Manager Console** (`Tools` \> `NuGet Package Manager` \> `Package Manager Console`) và chạy lệnh sau để cài đặt lại trình biên dịch:
    ```powershell
    Update-Package -reinstall Microsoft.CodeDom.Providers.DotNetCompilerPlatform
    ```

-----

#### Bước 5: Khởi Chạy Ứng Dụng

  * Nhấn `F5` hoặc nút **Start** để chạy dự án.

Khi chạy lần đầu, Entity Framework sẽ tự động tạo ra hai database (`WebBookStoreDb` và `WebBookStore_Users`) trên SQL Server của bạn và `DbInitializer` sẽ thêm vào các sản phẩm mẫu.

## Tích hợp giao diện

Dự án đã được nâng cấp giao diện cho trang chi tiết sản phẩm (`/Books/Details/{id}`).

  * **Tài nguyên:** Các file CSS (`global.css`, `index.css`) và hình ảnh (`.png`, `.svg`) từ thiết kế Figma/Locofy đã được thêm vào thư mục `Content/`.
  * **Cập nhật dữ liệu mẫu:** File `Data/DbInitializer.cs` đã được chỉnh sửa để tham chiếu chính xác đến các file hình ảnh có trong dự án, đảm bảo sản phẩm hiển thị đúng hình ảnh.
  * **Nâng cấp View:** File `Views/Books/Details.cshtml` đã được viết lại, kết hợp giữa cấu trúc HTML mới và các tính năng cũ (đánh giá, wishlist,...) để tạo ra một trải nghiệm người dùng thống nhất và hiện đại hơn.
