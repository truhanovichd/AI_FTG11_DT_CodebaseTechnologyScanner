# TechScanner UI (Frontend)

Frontend application for the **Codebase Technology Scanner** course project.

This app lets users:

- enter a local directory path,
- send a scan request to the backend API,
- view scan results in a dedicated results page.

The frontend is intentionally simple and readable (course-level complexity).

## Stack

- React 19 + TypeScript
- Vite
- React Router
- Vitest + React Testing Library

## Getting Started

### Prerequisites

- Node.js 18+
- npm 9+

### Install dependencies

```bash
npm install
```

### Start development server

```bash
npm run dev
```

Default local URL:

```text
http://localhost:5173
```

## Available Scripts

- `npm run dev` — run Vite dev server
- `npm run build` — type-check and build production bundle
- `npm run preview` — preview built app
- `npm run lint` — run ESLint
- `npm run test` — run Vitest (watch mode)
- `npm run test -- --run` — run tests once (CI style)
- `npm run test:ui` — open Vitest UI
- `npm run test:coverage` — run tests with coverage

## Project Structure (high-level)

- `src/layouts` — main/admin layout components
- `src/pages/main` — public pages (`Home`, `Scan`, `Results`)
- `src/pages/admin` — admin placeholder pages
- `src/test` — test setup and helpers

## Frontend Best Practices Used in This Project

- Keep components focused and small.
- Use explicit TypeScript types for key models and props.
- Handle loading, success, and error states for async calls.
- Use router-aware test wrappers (`MemoryRouter`) for navigation-dependent components.
- Prefer stable Testing Library queries (`getByRole`, `findByRole`) over brittle selectors.
- Reset storage and mocks between tests for deterministic results.

## API Contract Notes

- Frontend sends scan requests to `/api/scan`.
- Results are temporarily stored in `sessionStorage` for the results page flow.
- Ensure backend API is running and CORS allows `http://localhost:5173`.

## Troubleshooting

- If API calls fail in development, verify backend is running and reachable.
- If tests hang, prefer one-shot run:

```bash
npm run test -- --run
```

- If dependency resolution fails, check `.npmrc` and reinstall:

```bash
rm -rf node_modules package-lock.json
npm install
```
