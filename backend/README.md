# Codebase Technology Scanner API (Backend)

Backend Web API for the **Codebase Technology Scanner** course project.

This API scans a provided directory and returns technology-related files (for example `.csproj`, `package.json`, and `Dockerfile`).

The backend is intentionally lightweight and easy to read.

## Stack

- ASP.NET Core Web API (.NET 10)
- C# (latest language version available with SDK)
- Swagger / OpenAPI
- xUnit + FluentAssertions + Moq (tests)

## Getting Started

### Prerequisites

- .NET SDK 10

### Run API

From repository root:

```bash
dotnet run --project backend/src/CodebaseTechnologyScanner/CodebaseTechnologyScanner.csproj
```

Swagger UI is available in development at application root.

Health endpoint:

```text
/health
```

## Build and Test

### Build

```bash
dotnet build backend/src/CodebaseTechnologyScanner/CodebaseTechnologyScanner.csproj
```

### Run tests

```bash
dotnet test backend/tests/CodebaseTechnologyScanner.Tests/CodebaseTechnologyScanner.Tests.csproj
```

## Project Structure (high-level)

- `backend/src/CodebaseTechnologyScanner/Controllers` — API endpoints
- `backend/src/CodebaseTechnologyScanner/Services` — scanning logic and service contracts
- `backend/src/CodebaseTechnologyScanner/Models` — request/response and domain models
- `backend/tests/CodebaseTechnologyScanner.Tests` — unit and integration tests

## Backend Best Practices Used in This Project

- Keep services small and task-focused.
- Use dependency injection for service boundaries.
- Validate incoming request data and return clear errors.
- Prefer async patterns for I/O operations.
- Use structured logging via `ILogger<T>` when adding new operational logic.
- Keep architecture simple and course-friendly (no unnecessary layers).

## CORS and Frontend Integration

Current CORS setup allows:

```text
http://localhost:5173
```

This matches the default Vite development URL for the frontend app.

## Troubleshooting

- If frontend cannot call API, verify backend is running and CORS origin is correct.
- If Swagger is not visible, ensure `ASPNETCORE_ENVIRONMENT=Development`.
- If path scanning fails, verify OS path format and access permissions.
