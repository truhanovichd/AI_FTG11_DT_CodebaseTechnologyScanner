# Frontend Testing Setup

This directory contains the test configuration and setup files for the TechScanner UI.

## Test Framework

- **Vitest** - Fast, ESM-native unit test framework
- **@testing-library/react** - React component testing utilities
- **@testing-library/user-event** - User interaction simulation
- **jsdom** - JavaScript implementation of web standards for Node.js

## Test Configuration

- `setup.ts` - Test environment setup, mocks, and global configurations
- `vitest.config.ts` - Vitest configuration file (should be at project root)

## Running Tests

```bash
# Run tests
npm test

# Watch mode
npm test -- --watch

# UI mode
npm run test:ui

# Coverage
npm run test:coverage
```

## Test Files

Component tests are co-located with components using `.test.tsx` suffix:

- `src/pages/Home.test.tsx` - Home page tests
- `src/pages/Scan.test.tsx` - Scan page tests with form interaction
- `src/pages/Results.test.tsx` - Results page tests with mocked sessionStorage

## Mocking Strategy

- **Fetch API** - Mocked for testing API calls
- **sessionStorage** - Mocked for testing data persistence
- **window.matchMedia** - Mocked for responsive behavior tests

## Test Organization

- Tests use Vitest's `describe()` and `it()` functions
- FlexibleROLE and semantic queries from Testing Library
- User events simulated with `@testing-library/user-event`
- Router context provided via `<BrowserRouter>` wrapper

## Coverage Goals

- Component rendering tests
- User interaction tests
- API integration tests
- Error state handling
- Edge cases and empty states
