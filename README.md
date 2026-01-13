# 🕶️ Glass Store Management System

Dự án quản lý cửa hàng kính mắt được xây dựng trên nền tảng .NET Core theo kiến trúc phân lớp (Layered Architecture).

## 🏗️ Cấu trúc dự án (Project Structure)
Dự án được chia thành các project con để đảm bảo tính tách biệt (Separation of Concerns):

* **glassStore.Entities.NamNH**: Chứa các Model và thực thể của hệ thống.
* **glassStore.Repositories.NamNH**: Tầng truy xuất dữ liệu (Data Access Layer), làm việc trực tiếp với Database.
* **glassStore.Service.NamNH**: Tầng xử lý nghiệp vụ (Business Logic Layer), kết nối giữa Repositories và MVC.
* **glassStore.MVCWebApp.NamNH**: Tầng giao diện người dùng (User Interface) sử dụng mô hình ASP.NET Core MVC.

## 🛠️ Công nghệ sử dụng
* **Language:** C# (.NET)
* **Framework:** ASP.NET Core MVC
* **Database:** SQL Server (Scripts nằm trong thư mục `/Database`)

## 🚀 Hướng dẫn cài đặt

1.  **Clone dự án:**
    ```bash
    git clone [https://github.com/EricN2907/PRN222_glassStore.git](https://github.com/EricN2907/PRN222_glassStore.git)
    ```
2.  **Cấu hình Database:**
    * Chạy script SQL trong thư mục `Database` để tạo bảng và dữ liệu mẫu.
    * Tạo file `appsettings.json` trong project **glassStore.MVCWebApp.NamNH** (vì file này đã bị chặn bởi `.gitignore`).
    * Thêm chuỗi kết nối (Connection String) của bạn vào file vừa tạo.
3.  **Build & Run:**
    * Mở file `.sln` bằng Visual Studio.
    * Nhấn `F5` hoặc chọn `Run` để khởi động ứng dụng.

