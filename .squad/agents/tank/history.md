# Tank — History

## Project Context

- **Project:** azure-demos-agents-from-scratch-to-enterprise
- **My job:** Playwright end-to-end tests for the Vue.js search app in `src/vue-app/`
- **App URL:** Served by `npm run dev` (default: http://localhost:5173)
- **API URL:** https://localhost:5066 (via VITE_API_BASE_URL)
- **Test cases:** search happy path, validation (empty/short query), loading state, results display
- **User:** Bojan Vrhovnik

## Learnings

### Playwright E2E tests for Vue.js search app

- **Tests written:** 12
- **Test command:** `cd src/vue-app && npx playwright test --reporter=list`
- **Issues found and fixed:** None — all 12 tests passed on the first run.
- **Coverage:** page load, validation (empty/whitespace/single-char/clear-on-type), results display, result card fields (source/text/link), empty state, error state, loading/disabled state, Enter-key submit.
- **Mocking:** All tests use `page.route('**/basic/search*')` — no real backend required.
- **Files created:**
  - `src/vue-app/playwright.config.ts` — Playwright config with Vite dev server auto-start
  - `src/vue-app/tests/search.spec.ts` — 12 tests using Page Object Model (`SearchPage` class)
