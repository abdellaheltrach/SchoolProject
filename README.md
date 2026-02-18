# 🏫 School Management System

A robust, scalable, and high-performance backend solution built with **ASP.NET Core 10**, demonstrating modern software engineering practices and clean architecture.

---

## 1️⃣ Architectural Overview

The project implements a **Clean Architecture** combined with the **CQRS (Command Query Responsibility Segregation)** pattern.

- **Pattern Implementation**: Uses **MediatR** to decouple the presentation layer from the business logic.
- **Organization**: Concentric layers where dependencies flow inward, ensuring core domain logic is independent of external frameworks.

## 2️⃣ Layered Breakdown

### **Presentation Layer (`School.Api`)**

- **Responsibility**: Entry point for API requests, HTTP protocol handling, and response formatting.
- **Key Components**: RESTful Controllers, Middleware (Error Handling, Localization, Rate Limiting).
- **Tech**: ASP.NET Core Web API, Swagger/OpenAPI, Serilog.

### **Application Layer (`School.Core`)**

- **Responsibility**: Orchestrates business logic via CQRS. Contains use-case specific logic.
- **Key Components**: MediatR Commands/Queries, Validation logic, AutoMapper profiles.
- **Patterns**: CQRS, Pipeline Behaviors (Validation/Logging).

### **Service Layer (`School.Service`)**

- **Responsibility**: Reusable business services and complex domain logic.
- **Key Components**: `AuthenticationService`, `AuthorizationService`, `StudentService`.

### **Infrastructure Layer (`School.Infrastructure`)**

- **Responsibility**: Data access and external integrations.
- **Key Components**: `AppDbContext`, Repositories, Unit of Work, Seeders.
- **Patterns**: Repository Pattern (Generic & Specific), Unit of Work.
- **Tech**: EF Core, SQL Server, Microsoft Identity.

### **Domain Layer (`School.Domain`)**

- **Responsibility**: Core enterprise logic and entities.
- **Key Components**: POCO Entities (`Student`, `Instructor`, `Department`), Identity Entities (`User`, `Role`).

## 3️⃣ Security Implementation

- **Auth**: **JWT Bearer Tokens** + **ASP.NET Core Identity** with Refresh Token support.
- **Authorization**: **Policy-based** and **Claims-based RBAC**.
- **Protection**:
  - **Rate Limiting**: Custom per-IP (anonymous) and per-User (authenticated) limits.
  - **Validation**: Strict input validation using FluentValidation.
- **Auditing**: Structured logging via **Serilog** to SQL Server/Console.

## 4️⃣ Data & Persistence

- **Storage**: **SQL Server** for relational data.
- **ORM**: **EF Core** (Code-First) with automated migrations and seeding.
- **Transactions**: Managed via **Unit of Work** for ACID compliance across repositories.

## 5️⃣ Testing Strategy

- **Unit Testing**: Comprehensive tests for handlers and services using **xUnit** and **Moq**.
- **Integration Testing**: Database testing using **SQLite In-Memory**.
- **Tooling**: **FluentAssertions**, **Coverlet** (Coverage), **MockQueryable**.

## 6️⃣ DevOps / Deployment

- **Containerization**: Fully **Dockerized** with multi-stage builds.
- **Orchestration**: `docker-compose.yml` for environment-ready deployment.
- **Configuration**: Environment-specific settings via `appsettings.json`.

## 7️⃣ Engineering Concepts

- **SOLID Principles**: Strictly observed throughout the codebase.
- **DRY & Clean Code**: Centralized logic, meaningful naming, and localization support.
- **Scalability**: Stateless API design ready for horizontal scaling.
