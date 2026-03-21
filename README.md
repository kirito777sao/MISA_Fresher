# MISA_Fresher
Project Test Fresher MISA
# 🚀 MISA_Fresher

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Build](https://img.shields.io/badge/build-passing-brightgreen.svg)]()
[![Version](https://img.shields.io/badge/version-1.0.0-orange.svg)]()
[![Stack](https://img.shields.io/badge/stack-Vue%20%7C%20ASP.NET%20Core-lightgrey.svg)]()

Ứng dụng **MISA Fresher** – dự án thực hành trong chương trình đào tạo Fresher MISA, gồm **Frontend (Vue 3 + Vite)** và **Backend (ASP.NET Core 8)**.  
Phân hệ chính: **Ứng viên - AMIS Tuyển dụng**, hỗ trợ quản lý danh sách ứng viên, thêm/sửa thông tin, tìm kiếm nhanh, phân trang và lưu trữ dữ liệu bằng LocalStorage.

---

## 📚 Mục lục
- [Giới thiệu](#-giới-thiệu)
- [Yêu cầu môi trường](#-yêu-cầu-môi-trường)
- [Cài đặt](#-cài-đặt)
- [Cách sử dụng](#-cách-sử-dụng)
- [Cấu trúc thư mục](#-cấu-trúc-thư-mục)
- [Ảnh minh họa](#-ảnh-minh-họa)
- [Chức năng chính](#-chức-năng-chính)
- [Công nghệ sử dụng](#-công-nghệ-sử-dụng)
- [Đóng góp](#-đóng-góp)
- [License](#-license)

---

## 📖 Giới thiệu
Dự án mô phỏng phân hệ **Ứng viên** trong hệ thống AMIS Tuyển dụng của MISA.  
Các tính năng chính:
- Hiển thị danh sách ứng viên với **Header, Sidebar, DataTable**.
- **Phân trang, tìm kiếm nhanh** theo tên, email, số điện thoại.
- Dữ liệu ứng viên được khai báo trong `candidate-data.js/json` và lưu vào **LocalStorage**.
- Cho phép **thu gọn/mở rộng Sidebar** và lưu trạng thái.
- Popup **Thêm mới ứng viên** (insert đầu danh sách).
- Popup **Chỉnh sửa ứng viên** (bind dữ liệu, cập nhật LocalStorage).
- Hiển thị **toast thông báo** khi thao tác thành công/thất bại.

---

## 🛠️ Yêu cầu môi trường
- Node.js >= 18
- .NET 8 SDK
- SQL Server (cho backend)
- Visual Studio 2022 / VS Code

🧩 Chức năng chính
📋 Danh sách ứng viên: hiển thị, phân trang, tìm kiếm nhanh.

➕ Thêm ứng viên: popup nhập thông tin, lưu vào LocalStorage.

✏️ Sửa ứng viên: bind dữ liệu, cập nhật LocalStorage.

📦 LocalStorage: lưu trạng thái sidebar, dữ liệu ứng viên.

🔔 Toast thông báo: hiển thị kết quả thao tác.

💻 Công nghệ sử dụng
Frontend: Vue 3, Vite, ESLint, Prettier

Backend: ASP.NET Core 8, Entity Framework Core

Database: SQL Server

Version Control: Git, GitHub

🤝 Đóng góp
Fork repo

Tạo branch mới: feature/my-feature

Commit & push

Tạo Pull Request

---

## ⚙️ Cài đặt
```bash
# Clone project
git clone https://github.com/kirito777sao/MISA_Fresher.git
cd MISA_Fresher

# Frontend (Vue 3 + Vite)
cd MISA_Fresher_FE
npm install
npm run dev

# Backend (ASP.NET Core 8)
cd MISA_Fresher_BE/MISA.Fresher.Api
dotnet restore
dotnet run

MISA_Fresher/
├── MISA_Fresher_BE/
│   ├── MISA.Fresher.Api/           # API controllers, Program.cs, appsettings.json
│   ├── MISA.Fresher.Core/          # Entities, DTOs, Services, Interfaces, Middlewares
│   └── MISA.Fresher.Infrastructure # Repositories, Mappings
│
├── MISA_Fresher_FE/
│   ├── src/                        # Vue components, pages, services
│   ├── public/                     # Static assets
│   ├── vite.config.js              # Vite config
│   └── package.json                # Dependencies
│
└── README.md

