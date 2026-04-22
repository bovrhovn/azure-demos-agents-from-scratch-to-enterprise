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

### Full test run — all suites (2025-07-17)

- **.NET unit tests (ASE.Libraries.Tests):** 51/51 passed, 0 failed, 0 skipped. Duration: ~8.3s.
- **Playwright E2E tests (chat-web-app):** 12/12 passed, 0 failed. Duration: ~35.8s.
- **Note:** Playwright tests now live in `src/chat-web-app/` (not `src/vue-app/`).
- **Overall status:** ✅ All 63 tests green.
