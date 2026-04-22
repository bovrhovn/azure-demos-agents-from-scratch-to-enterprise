# Squad Decisions

## Active Decisions

### 2025-07-10: Vue app structure decided
**By:** Trinity
**What:** Created Vue 3 + TypeScript + Vite + Vue Router + Tailwind CSS app in `src/vue-app/`. API service calls `GET {VITE_API_BASE_URL}/basic/search?query={query}`. All components use `data-testid` attributes for Playwright. Search validation: required, min 2 chars.
**Why:** Per user requirement — Bojan Vrhovnik

### 2025-07-16: CORS configured for Vue.js frontend
**By:** Morpheus
**What:** Added CORS policy "AllowVueApp" to ASE.EnterpriseApi allowing origins http/https://localhost:5173, 4173, 3000. Middleware registered before route mapping.
**Why:** Vue.js app running on Vite dev server (port 5173) needs to call the API (port 5066). Browser enforces CORS.

### 2025-01-30: Playwright tests passing
**By:** Tank
**What:** 12 E2E tests covering: page load, validation (empty/whitespace/single-char/clear-on-type), results display (source name, text, link), empty state, error state, loading/disabled state, Enter-key submit. All tests use page.route() mocking — no real backend needed.
**Why:** Requested by Bojan Vrhovnik

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction
