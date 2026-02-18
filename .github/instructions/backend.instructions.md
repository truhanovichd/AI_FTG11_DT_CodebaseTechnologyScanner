# Backend Instructions — ASP.NET Core (.NET 10)

## Architecture & Style

- Keep architecture lightweight and course-appropriate.
- Follow existing project structure and naming conventions.
- Use interfaces for service boundaries when it improves testability.
- Prefer primary constructor syntax for dependency injection when practical.

## C# and API Guidelines

- Add XML documentation for public classes, methods, and properties.
- Use explicit request/response models for controller actions.
- Validate incoming request data and fail fast with clear errors.
- Use async/await for I/O operations and return Task/Task<T>.
- Use structured logging via `ILogger<T>`.
- Throw specific exceptions with meaningful messages.
- Always use braces for all control flow statements.

## Dependency Injection

- Register services with appropriate lifetimes (Scoped/Singleton/Transient).
- Guard constructor dependencies against null values.
- Keep service constructors minimal and avoid hidden side effects.

## Configuration

- Use strongly-typed options for configuration where applicable.
- Keep `appsettings.json` and environment-specific overrides aligned.
- Avoid hard-coded environment-specific values in code.

## Testing Expectations

- Use MSTest for backend tests.
- Prefer FluentAssertions for expressive assertions.
- Follow AAA pattern (Arrange, Act, Assert).
- Cover both successful and failure scenarios.
- Add null/invalid input validation tests for public entry points.

## Pragmatic Boundaries

- Do not add enterprise patterns unless they solve a real current need.
- Keep handlers/services short and cohesive.
- Prioritize maintainability for student team collaboration.
