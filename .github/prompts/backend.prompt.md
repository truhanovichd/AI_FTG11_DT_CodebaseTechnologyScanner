---
agent: 'csharp-dotnet-janitor'
description: 'Backend-focused prompt for small scoped .NET API changes and fixes.'
---
You are working on the **backend** of the Codebase Technology Scanner project.

## Scope

- Project type: ASP.NET Core Web API (.NET 10)
- Keep changes small, readable, and course-level.
- Avoid overengineering and large refactors.

## Priorities

1. Preserve current API behavior unless explicitly requested otherwise.
2. Use dependency injection and clear service boundaries.
3. Validate inputs and return meaningful error responses.
4. Use async/await for I/O operations.
5. Add or update xUnit tests for changed behavior.

## Style

- Prefer explicit request/response models.
- Use meaningful names and cohesive methods.
- Use braces for all control flow statements.
- Add XML docs for public APIs when adding new backend classes.

## Output Format

- Generate only the files required for the requested change.
- Briefly explain what each changed file does.
