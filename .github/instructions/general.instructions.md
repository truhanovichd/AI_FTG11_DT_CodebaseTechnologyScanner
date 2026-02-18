# General Instructions — Codebase Technology Scanner

## Project Scope

- This is a course-level project, not a production system.
- Keep implementation simple, readable, and easy to explain.
- Avoid overengineering, unnecessary abstractions, and speculative patterns.
- Prefer small, scoped changes over large rewrites.

## Tech Stack Context

- Backend: ASP.NET Core Web API (.NET 10, C# latest)
- Frontend: React + Vite + TypeScript
- App structure includes multiple pages, layouts, and routing.

## AI-First Development Style

- The codebase is expected to be primarily AI-assisted.
- Generate small, focused files and changes.
- For each new file, include a brief purpose description in PR notes or commit notes.

## Coding Principles

- Prefer clarity over cleverness.
- Keep methods/components focused on a single responsibility.
- Use meaningful domain-oriented names.
- Reuse existing patterns before introducing new ones.
- Always use braces for control flow blocks.

## Quality & Safety

- Validate inputs at boundaries (API requests, forms, route params).
- Handle expected errors explicitly with user-friendly messages.
- Add tests for new logic or bug fixes when test infrastructure already exists.
- Do not introduce unrelated refactors while fixing a specific issue.

## Change Discipline

- Keep commits focused and atomic.
- Preserve existing behavior unless a task explicitly requests change.
- Update docs or comments when behavior, API contracts, or setup changes.
- Prefer deterministic tests and avoid flaky timing-based checks where possible.
