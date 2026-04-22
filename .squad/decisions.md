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

### 2025-07-16: scripts/ — run-all, build-acr, run-tests
**By:** Neo
**What:** Created `scripts/` directory with three PowerShell operational scripts: `run-all.ps1` (starts backend + frontend in separate PS windows), `build-acr.ps1` (builds Docker images via `az acr build` without local Docker), `run-tests.ps1` (runs .NET unit tests and Playwright E2E tests). Backend context is `src/AgentScratchEnterprise/` with frontend `VITE_API_BASE_URL` passed as build-arg.
**Why:** Developers need single entry point for full stack; CI/CD requires reproducible way to build for ACR without Docker Desktop; test automation needs unified script covering backend (xunit) and frontend (Playwright).

### 2025-07-16: Frontend Dockerfile & nginx config
**By:** Trinity
**What:** Added production container support for `src/chat-web-app`: multi-stage Dockerfile (`node:20-alpine` build, `nginx:alpine` serve), SPA-friendly nginx config with `try_files $uri $uri/ /index.html` routing, default `VITE_API_BASE_URL=http://localhost:5066`, `/health` endpoint for health checks. Also added `scripts/run-frontend.ps1` for local dev.
**Why:** Vue.js frontend needs containerization for deployment; Vite bakes env vars at build time, so `VITE_API_BASE_URL` must be provided as `--build-arg`. Multi-stage build keeps final image minimal.

### 2025-07-17: Dockerfile fix and run scripts
**By:** Morpheus
**What:** Fixed `ASE.EnterpriseApi/Dockerfile` — changed broken `COPY` pattern to two-step approach copying both `ASE.EnterpriseApi.csproj` and `ASE.Libraries.csproj` before restore (build context is `AgentScratchEnterprise/` where libraries is sibling project). Created `scripts/run-backend.ps1` for local API startup on port 5066. Added `src/chat-web-app/.dockerignore` to exclude `node_modules`, `dist`, test artifacts, `.env` from Docker build context.
**Why:** Original Dockerfile `dotnet restore` failed in CI/Docker because `ASE.Libraries.csproj` was never available at restore time. Run script provides consistent way to start backend locally without manual env var setup. Dockerignore keeps builds fast and prevents secrets from baking into images.

### 2025-07-17: Test Results — All 63 tests passing
**By:** Tank
**What:** .NET unit tests (ASE.Libraries.Tests): 51 passed in ~8.3s. Playwright E2E tests (chat-web-app): 12 passed in ~35.8s covering load, validation, results display, empty state, error handling, loading state, Enter-key submit. Total: 63 passed, 0 failed, 0 skipped.
**Why:** Validates that backend API changes and frontend implementation work correctly. Confirms all 12 E2E test scenarios and 51 unit tests pass.

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction
