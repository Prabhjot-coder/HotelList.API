# HotelList API — EF Core DB-First Approach

> Branch: `PracticeEFcoreWithDbFirstApproach`

A production-ready ASP.NET Core 8 Web API using **EF Core Database-First**, **Generic Repository Pattern**, **JWT Authentication**, **Options Pattern**, and **xUnit tests**.

---

## 🏗️ Architecture

```
WebApplication4/
├── Configuration/          # Options pattern
│   ├── JwtSettings.cs
│   └── AppSettings.cs
├── Data/
│   └── SchoolContext.cs    # EF Core DB-First DbContext
├── Models/                 # DB-First entity models
│   ├── Course.cs
│   ├── Department.cs
│   ├── Student.cs
│   └── Teacher.cs
├── DTOs/                   # Request / Response DTOs
│   ├── AuthDto.cs
│   ├── CourseDto.cs
│   ├── DepartmentDto.cs
│   ├── StudentDto.cs
│   └── TeacherDto.cs
├── Repositories/           # Generic Repository Pattern
│   ├── IGenericRepository.cs
│   ├── GenericRepository.cs
│   ├── ICourseRepository.cs / CourseRepository.cs
│   ├── IDepartmentRepository.cs / DepartmentRepository.cs
│   ├── IStudentRepository.cs / StudentRepository.cs
│   └── ITeacherRepository.cs / TeacherRepository.cs
├── Services/               # Business services
│   ├── ITokenService.cs
│   └── TokenService.cs     # JWT generation
├── Controllers/
│   ├── AuthController.cs   # Register + Login (JWT)
│   ├── CoursesController.cs
│   ├── DepartmentsController.cs
│   ├── StudentsController.cs
│   └── TeachersController.cs
├── appsettings.json                # Base config
├── appsettings.Development.json    # Dev overrides
├── appsettings.Staging.json        # Staging overrides
├── appsettings.Production.json     # Prod overrides
└── Program.cs              # DI, JWT, Swagger wiring

HotelList.API.Tests/
├── HotelList.API.Tests.csproj      # xUnit + FluentAssertions + Moq
├── Repositories/
│   └── GenericRepositoryTests.cs   # 10 in-memory DB tests
└── Controllers/
    ├── AuthControllerTests.cs       # 7 unit tests
    ├── CoursesControllerTests.cs    # 5 unit tests
    └── StudentsControllerTests.cs   # 9 unit tests
```

---

## 🔑 JWT Authentication

### Register
```http
POST /api/auth/register
Content-Type: application/json

{ "firstName": "John", "lastName": "Doe", "email": "john@example.com", "password": "P@ssword1!" }
```

### Login
```http
POST /api/auth/login
Content-Type: application/json

{ "email": "john@example.com", "password": "P@ssword1!" }
```

Returns:
```json
{ "token": "<jwt>", "email": "john@example.com", "fullName": "John Doe", "expiresAt": "..." }
```

Use the token as: `Authorization: Bearer <token>` on all protected endpoints.

---

## 📦 CRUD Endpoints

All endpoints require `Authorization: Bearer <token>` header.

| Method | Route | Description |
|--------|-------|-------------|
| GET    | /api/students | Get all students |
| GET    | /api/students/{id} | Get student by ID |
| GET    | /api/students/by-course/{courseId} | Students by course |
| POST   | /api/students | Create student |
| PUT    | /api/students/{id} | Update student |
| DELETE | /api/students/{id} | Delete student |
| GET    | /api/courses | Get all courses |
| GET    | /api/courses/active | Active courses only |
| GET    | /api/courses/{id} | Get course by ID |
| POST   | /api/courses | Create course |
| PUT    | /api/courses/{id} | Update course |
| DELETE | /api/courses/{id} | Delete course |
| GET    | /api/departments | Get all departments |
| GET    | /api/departments/active | Active departments |
| POST   | /api/departments | Create department |
| PUT    | /api/departments/{id} | Update department |
| DELETE | /api/departments/{id} | Delete department |
| GET    | /api/teachers | Get all teachers |
| GET    | /api/teachers/by-department/{id} | Teachers by dept |
| POST   | /api/teachers | Create teacher |
| PUT    | /api/teachers/{id} | Update teacher |
| DELETE | /api/teachers/{id} | Delete teacher |

---

## ⚙️ Options Pattern

Settings are bound via `IOptions<T>` — no magic strings:

```csharp
// In any service or controller:
public MyService(IOptions<JwtSettings> jwt, IOptions<AppSettings> app)
{
    var key = jwt.Value.Key;
    var appName = app.Value.ApplicationName;
}
```

---

## 🧪 Running Tests

```bash
cd HotelList.API.Tests
dotnet test --logger "console;verbosity=detailed"
```

**31 total test cases** across:
- `GenericRepositoryTests` — 10 cases (in-memory EF Core)
- `StudentsControllerTests` — 9 cases (Moq)
- `AuthControllerTests` — 7 cases (Moq)
- `CoursesControllerTests` — 5 cases (Moq)

---

## 🚀 Running the API

```bash
# Development
dotnet run --environment Development

# Staging
dotnet run --environment Staging

# Swagger UI available at:
# https://localhost:{port}/swagger
```
