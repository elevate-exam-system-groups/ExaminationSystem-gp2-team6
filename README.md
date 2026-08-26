# Examination System API

This project is a robust, scalable Web API for an Examination System. It is built using **.NET 8** and adheres to the principles of **Clean Architecture** (specifically utilizing a Vertical Slice/Features-based approach).

## 🚀 Features

The system is organized into distinct feature modules:

- **AdminManagement:** Tools and endpoints for administrative tasks.
- **Authentication:** Secure user login and registration using ASP.NET Core Identity and JWT Bearer Tokens.
- **Diplomas:** Management of diplomas/courses.
- **QuizEngine:** The core engine for handling quizzes, attempts, and result calculations.
- **StudentDashboard:** Specialized endpoints for the student experience, tracking progress and available quizzes.
- **Common:** Shared functionality and cross-cutting concerns.

## 🏗️ Architecture & Technologies

- **Framework:** .NET 8 (ASP.NET Core Web API)
- **Architecture:** Clean Architecture / Vertical Slice Architecture
- **Data Access:** Entity Framework Core 8 with SQL Server
- **Authentication:** ASP.NET Core Identity & JWT Authentication
- **CQRS Pattern:** MediatR for handling commands and queries
- **Validation:** FluentValidation
- **Object Mapping:** AutoMapper
- **API Documentation:** Swagger / OpenAPI

## 📚 Domain Models (Entities)

The core domain encapsulates entities such as:
- **Users & Access:** `User`, `Email`, `PasswordResetTemporaryToken`
- **Academics:** `Diploma`, `Quiz`, `Question`, `AnswerOption`
- **Assessment:** `QuizAttempt`, `AttemptAnswer`, `AttemptResult`

## ⚙️ Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (or LocalDB)

### Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   ```

2. **Configure Database Connection:**
   Update the `appsettings.json` file in the `ExaminationSystem` project with your local SQL Server connection string.

3. **Apply Migrations and Run:**
   You can run the project using Visual Studio, Rider, or the .NET CLI.
   ```bash
   cd ExaminationSystem
   dotnet build
   dotnet run
   ```
   The application uses a database seeder (`IDataSeeding`) to populate initial roles, admin users, and necessary seed data upon startup.

4. **Explore the API:**
   Navigate to the Swagger UI in your browser (typically `https://localhost:<port>/swagger` in Development mode) to explore and test the available endpoints.