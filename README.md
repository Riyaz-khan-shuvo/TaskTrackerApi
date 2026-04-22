
# 📡 TaskTrackerApi – ASP.NET Core Web API

## 📌 Overview

TaskTrackerApi is a RESTful Web API built using **ASP.NET Core (.NET 8)** to manage tasks for the Task Tracker system. It provides secure endpoints for authentication and task management, designed to be consumed by the MVC frontend (TaskTrackerUI).

---

## ⚙️ Technology Stack

* ASP.NET Core Web API (.NET 8)
* Entity Framework Core
* ASP.NET Core Identity
* MS SQL Server
* JWT / Cookie-based Authentication (based on implementation)
* Clean Architecture (Controller → Service → Repository)

---

## 🗄️ Database Configuration

This project uses **two separate databases**:

### 🔐 Authentication Database

```json
"TaskTrackerAuthConnection": "Server=DESKTOP-T0BS21J\\SQLEXPRESS;Database=TaskTrackerAuthDb;Integrated Security=true;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

### 📋 Application Database

```json
"TaskTrackerConnection": "Server=DESKTOP-T0BS21J\\SQLEXPRESS;Database=TaskTrackerDb;Integrated Security=true;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

---

## 🚀 Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/Riyaz-khan-shuvo/TaskTrackerApi.git
```

---

### 2. Update Connection Strings

Open `appsettings.json` and update the connection strings according to your SQL Server setup:

```json
{
  "ConnectionStrings": {
    "TaskTrackerConnection": "Server=DESKTOP-T0BS21J\\SQLEXPRESS;Database=TaskTrackerDb;Integrated Security=true;MultipleActiveResultSets=true;TrustServerCertificate=True",
    "TaskTrackerAuthConnection": "Server=DESKTOP-T0BS21J\\SQLEXPRESS;Database=TaskTrackerAuthDb;Integrated Security=true;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

> ⚠️ Make sure SQL Server is running and the instance name is correct.

---

### 3. Apply Database Migrations

Run the following commands in **Package Manager Console** or terminal:

#### For Main Database

```bash
dotnet ef database update --context ApplicationDbContext
```

#### For Auth Database

```bash
dotnet ef database update --context AuthDbContext
```

---

### 4. Run the Application

```bash
dotnet run
```

API will start at:

```
https://localhost:{port}
```

---

## 🔐 Authentication Setup

* ASP.NET Core Identity is used for user management
* Separate database ensures clean separation of concerns
* Secure password hashing handled by Identity framework
* `[Authorize]` attribute protects secured endpoints

---

## 📡 Main API Endpoints

### 🔑 Authentication

* POST `/api/account/register`
* POST `/api/account/login`
* POST `/api/account/logout`



## 🧠 Architecture Highlights

This project follows a **Clean Architecture approach** with strong separation of concerns and enterprise-level design patterns.

### 🏛️ Clean Architecture Structure

* Separation between **API, Application, Infrastructure, and Domain layers**
* Domain layer is independent of external frameworks
* Application layer handles business use cases
* Infrastructure layer handles database & external services
* API layer acts as the entry point (controllers only)

---

### ⚡ CQRS (Command Query Responsibility Segregation)

* Commands and Queries are separated for better scalability and maintainability
* Write operations handled through **Commands**
* Read operations handled through **Queries**
* Each request has a dedicated handler for single responsibility

---

### 📩 MediatR (In-Process Messaging)

* Used for decoupling controllers from business logic
* Controllers only send requests via `IMediator`
* Business logic handled inside **Request Handlers**
* Improves testability and maintainability

---

### 🗂 Repository Pattern

* Abstracts data access logic from business layer
* Generic + specific repositories used for flexibility
* Enables cleaner, reusable data access code
* Improves unit testing by isolating database logic

---

### 🔄 Unit of Work Pattern

* Ensures **single transaction consistency across multiple repositories**
* Manages commit/rollback operations centrally
* Prevents partial data updates
* Improves data integrity and reliability

---

### 💉 Dependency Injection (DI)

* Built-in ASP.NET Core DI container used
* Loose coupling between services, repositories, and handlers
* Easy to replace implementations without modifying core logic

---

### ⚙️ Async / Await Pattern

* Fully asynchronous API implementation
* Non-blocking database operations using EF Core async methods
* Improves performance and scalability under load

---

### 📈 Scalable Design Principles

* Feature-based folder structure
* Easily extendable for new modules
* Clear separation of concerns
* Follows SOLID principles
* Ready for enterprise-level scaling

---

## 🛠️ Common Setup Issue (Important)

If database is not created, run:

```bash
dotnet ef migrations add InitialCreate --context ApplicationDbContext
dotnet ef database update --context ApplicationDbContext
```

and for Auth:

```bash
dotnet ef migrations add InitialAuthCreate --context AuthDbContext
dotnet ef database update --context AuthDbContext
```

---

## 👨‍💻 Author

**Riyaz Hossain**
.NET Developer

---

