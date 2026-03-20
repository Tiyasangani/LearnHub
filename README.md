# LearnHub — ASP.NET Core MVC

A fully-featured online learning platform built with ASP.NET Core 10 MVC, Entity Framework Core (SQLite), Bootstrap 5, and clean production-ready code.

---

## 📁 Project Structure

```
LearnHub/
├── Controllers/
│   ├── HomeController.cs
│   ├── AccountController.cs
│   ├── CoursesController.cs
│   ├── PaymentController.cs
│   ├── QuizController.cs
│   ├── CertificateController.cs
│   ├── ContactController.cs
│   └── AdminController.cs
├── Models/
│   └── Models.cs              ← All Models + ViewModels
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   ├── _AdminLayout.cshtml
│   │   └── _CourseCard.cshtml
│   ├── Home/        Index, About
│   ├── Account/     Login, Register, ForgotPassword
│   ├── Courses/     Index
│   ├── Payment/     Checkout, Success
│   ├── Quiz/        Take, Result
│   ├── Certificate/ View
│   ├── Contact/     Index
│   └── Admin/       Index, ManageCourses, AddCourse, EditCourse, ManageUsers
├── Data/
│   ├── LearnHubDbContext.cs
│   └── DbSeeder.cs
├── Migrations/
│   ├── 20260101000000_InitialCreate.cs
│   └── LearnHubDbContextModelSnapshot.cs
├── wwwroot/
│   ├── css/   site.css, admin.css
│   ├── js/    site.js
│   └── images/
├── Program.cs
├── appsettings.json
└── LearnHub.csproj
```

---

## 🚀 Getting Started in VS Code

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- VS Code with **C# Dev Kit** extension

### 1 — Restore & Build

```bash
cd LearnHub
dotnet restore
dotnet build
```

### 2 — Database Setup

The app uses **SQLite** (zero-config). The database is auto-created on first run via `EnsureCreated()`.

To use EF Core migrations instead:

```bash
# Install EF tools (once)
dotnet tool install --global dotnet-ef

# Apply migrations
dotnet ef database update
```

### 3 — Run

```bash
dotnet run
```

Open your browser: **https://localhost:5001** (or the port shown in terminal)

---

## 🔐 Default Credentials

| Role  | Email                 | Password   |
|-------|-----------------------|------------|
| Admin | admin@learnhub.com    | admin123   |

Register a new account via the Sign Up page to test the student flow.

---

## 📄 Pages Overview

| Page                  | URL                           |
|-----------------------|-------------------------------|
| Home                  | /                             |
| Courses               | /Courses                      |
| Course by Category    | /Courses?category=Programming |
| Login                 | /Account/Login                |
| Register              | /Account/Register             |
| Forgot Password       | /Account/ForgotPassword       |
| Payment / Checkout    | /Payment/Checkout?courseId=1  |
| Quiz                  | /Quiz/Take?courseId=1         |
| Quiz Result           | /Quiz/Result                  |
| Certificate           | /Certificate/View?courseId=1  |
| Contact               | /Contact                      |
| Admin Dashboard       | /Admin                        |
| Admin — Manage Courses| /Admin/ManageCourses          |
| Admin — Add Course    | /Admin/AddCourse              |
| Admin — Manage Users  | /Admin/ManageUsers            |
| About                 | /Home/About                   |

---

## 🛠 Tech Stack

| Layer      | Technology                              |
|------------|-----------------------------------------|
| Framework  | ASP.NET Core 10 MVC                     |
| ORM        | Entity Framework Core 9 + SQLite        |
| Auth       | Session-based (swap to Identity easily) |
| Frontend   | Bootstrap 5.3, Bootstrap Icons          |
| Passwords  | BCrypt.Net-Next                         |

---

## 🔄 Switching to SQL Server

1. In `LearnHub.csproj`, replace the SQLite package with:
   ```xml
   <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
   ```
2. In `Program.cs`, replace `UseSqlite(...)` with:
   ```csharp
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
   ```
3. Update `appsettings.json` connection string to your SQL Server instance.

---

## 📌 Notes

- The seeder runs once on startup and seeds 8 courses + an admin user.
- Quiz questions are seeded for Python and Web Development courses.
- Certificate is automatically issued when a quiz is passed (≥60%).
- Admin guard uses session; for production, replace with ASP.NET Core Identity + roles.
