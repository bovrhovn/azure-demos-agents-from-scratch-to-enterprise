# Neo — History

## Project Context

- **Project:** azure-demos-agents-from-scratch-to-enterprise
- **Stack:** Vue 3 + TypeScript + Vite + Vue Router 4 + Tailwind CSS
- **User:** Bojan Vrhovnik
- **Goal:** Vue.js search UI calling `GET {baseUrl}/basic/search?query={query}` which returns `[{sourceName, sourceLink, text}]`
- **API Base URL:** `VITE_API_BASE_URL` env var (default: https://localhost:5066)
- **Source location:** `src/vue-app/`

## Learnings

### 2025-07-16: Created scripts/ directory with three operational scripts
**What:**
- `scripts/run-all.ps1` — launches backend (.NET 10) and frontend (Vite) in separate PowerShell windows
- `scripts/build-acr.ps1` — builds both Docker images via `az acr build` (cloud build, no local Docker needed); accepts `-AcrName`, `-ResourceGroup`, `-BackendTag`, `-FrontendTag`, `-ViteApiBaseUrl`, `-NoPush`
- `scripts/run-tests.ps1` — runs .NET xunit tests (`tests/ASE.Libraries.Tests`) and Playwright E2E tests (`src/chat-web-app`); supports `-SkipDotnet` / `-SkipPlaywright`

**Key decisions:**
- ACR build uses cloud-side build context; backend context is `src/AgentScratchEnterprise/` with Dockerfile at `ASE.EnterpriseApi/Dockerfile`
- Frontend image receives `VITE_API_BASE_URL` as a build-arg so the API URL is baked in at image build time
- `run-all.ps1` delegates to `run-backend.ps1` and `run-frontend.ps1` (sibling scripts, expected to be created by other agents)
