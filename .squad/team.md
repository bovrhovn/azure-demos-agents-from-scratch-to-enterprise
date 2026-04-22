# Squad Team

> azure-demos-agents-from-scratch-to-enterprise

## Coordinator

| Name | Role | Notes |
|------|------|-------|
| Squad | Coordinator | Routes work, enforces handoffs and reviewer gates. |

## Members

| Name | Role | Charter | Status |
|------|------|---------|--------|
| Neo | Lead | .squad/agents/neo/charter.md | active |
| Trinity | Frontend Dev | .squad/agents/trinity/charter.md | active |
| Morpheus | Backend/API | .squad/agents/morpheus/charter.md | active |
| Tank | Tester | .squad/agents/tank/charter.md | active |
| Scribe | Session Logger | .squad/agents/scribe/charter.md | active |
| Ralph | Work Monitor | — | active |

## Project Context

- **Project:** azure-demos-agents-from-scratch-to-enterprise
- **Stack:** Vue 3 + TypeScript + Vite + Vue Router + Tailwind CSS, Playwright tests
- **User:** Bojan Vrhovnik
- **Universe:** The Matrix
- **Created:** 2026-04-21
- **Task:** Build Vue.js search UI that calls `GET {baseUrl}/basic/search?query={query}` — returns `[{sourceName, sourceLink, text}]`. Default base URL: https://localhost:5066. Source in `src/vue-app/`.
- **API Base URL env var:** `VITE_API_BASE_URL`
