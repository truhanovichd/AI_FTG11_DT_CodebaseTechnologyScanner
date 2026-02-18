# Frontend Instructions — React + Vite

## Frontend Scope

- Build simple, clear UI flows for course-level requirements.
- Keep components small and easy to reason about.
- Prefer straightforward state management over complex abstractions.

## React & TypeScript Standards

- Use functional components with hooks.
- Use explicit TypeScript interfaces/types for props and key models.
- Keep component responsibilities focused (presentation vs data handling).
- Reuse existing layouts, pages, and routing patterns.

## UX and Behavior

- Show clear loading, success, and error states for async actions.
- Keep text and interactions predictable and consistent.
- Avoid adding extra UI complexity unless explicitly requested.

## API Integration

- Validate input before sending API requests.
- Handle non-OK HTTP responses and display friendly messages.
- Keep API response mapping simple and defensive.

## Testing (Vitest + React Testing Library)

- Use behavior-focused tests from the user perspective.
- Prefer stable queries (`getByRole`, `findByRole`, explicit text) over brittle selectors.
- Wrap router-dependent components in a router test wrapper (`MemoryRouter`).
- Avoid ambiguous matchers that can return multiple elements.
- Keep tests deterministic and independent (reset storage/mocks between tests).

## Maintainability

- Keep CSS and component structure aligned with existing project style.
- Avoid introducing new libraries unless required by a concrete task.
- Favor minimal, incremental changes that are easy to review.
