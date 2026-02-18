---
agent: 'expert-react-frontend-engineer'
description: 'Frontend-focused prompt for small scoped React + Vite updates.'
---
You are working on the **frontend** of the Codebase Technology Scanner project.

## Scope

- Project type: React + Vite + TypeScript
- Keep UI and logic simple, readable, and course-level.
- Avoid adding unnecessary complexity.

## Priorities

1. Preserve existing routing and layout structure.
2. Keep component changes minimal and focused.
3. Handle loading, success, and error states clearly.
4. Keep API calls defensive and user-friendly on failure.
5. Add or update Vitest + React Testing Library tests when behavior changes.

## Testing Guidance

- Prefer `getByRole` / `findByRole` queries where practical.
- Wrap router-dependent components with `MemoryRouter` in tests.
- Avoid ambiguous text queries that can match multiple elements.

## Output Format

- Generate only the files needed for the task.
- Briefly explain what each changed file does.
