# Clinic Management API

This is a RESTful API built with ASP.NET Core following Clean Architecture principles.
The purpose of this project is to demonstrate structured backend development, separation of concerns, and real-world API design using C# and SQL Server.

## Overview

The system manages core clinic operations such as patients, doctors, and appointments.
It is designed to be scalable, maintainable, and easy to extend.

The project emphasizes:

* Clear architectural boundaries
* Business logic isolation
* Proper use of DTOs
* Authentication and authorization
* Database relationships
* Clean and readable code

---

## Architecture

The solution follows Clean Architecture to separate responsibilities across layers.

```
ClinicManagement/
│
├── Domain/              → Entities and core business rules
├── Application/         → DTOs, Interfaces, Services logic
├── Infrastructure/      → Database context, Repositories
├── Clinic.API/          → Controllers, Middleware, Configuration
```

Each layer has a single responsibility:

* **Domain** contains enterprise business rules and entities.
* **Application** contains use cases and service abstractions.
* **Infrastructure** handles database access and external concerns.
* **ClinicAPI** exposes HTTP endpoints and handles configuration.

This structure allows the business logic to remain independent from frameworks and database implementation.

---

## Technologies Used

* .NET 8
* ASP.NET Core Web API
* C#
* SQL Server
* Entity Framework Core
* JWT Authentication
* Dependency Injection

---

## Features

* User registration and login with JWT authentication
* Role-based authorization
* CRUD operations for patients, doctors, and appointments
* DTO pattern to prevent domain exposure
* Input validation
* Global exception handling
* Structured API responses
* Relational database design with foreign keys

---

## Database Design

The system uses a relational model with the following relationships:

* One Doctor can have multiple Appointments
* One Patient can have multiple Appointments

Entity Framework Core migrations are used for schema management.

---

## How to Run

1. Clone the repository:

```bash
git clone https://github.com/Joelleon96/clinic-management-api.git
```

2. Configure the connection string inside `appsettings.json`.

3. Apply migrations:

```bash
dotnet ef database update
```

4. Run the application:

```bash
dotnet run
```

5. Access Swagger UI at:

```
https://localhost:5001/swagger
```

---

## What This Project Demonstrates

Through this project, I focused on applying:

* Clean Architecture principles
* SOLID design principles
* Dependency Injection
* Proper separation of concerns
* RESTful API standards
* Secure authentication practices
* Maintainable and scalable backend structure

---

## Future Improvements

* Unit testing implementation
* Integration testing
* Docker support
* CI/CD pipeline configuration
* Cloud deployment

---

## Author
Joel Leon
Backend-focused Software Engineer working with C# and ASP.NET Core.
This repository represents practical implementation of scalable API architecture and clean code practices.
