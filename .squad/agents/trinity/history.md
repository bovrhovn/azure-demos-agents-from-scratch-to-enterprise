# Trinity — History

## Project Context

- **Project:** azure-demos-agents-from-scratch-to-enterprise
- **Stack:** Vue 3 + TypeScript + Vite + Vue Router 4 + Tailwind CSS
- **User:** Bojan Vrhovnik
- **Goal:** Build Vue.js search UI in `src/vue-app/`
- **API endpoint:** `GET {VITE_API_BASE_URL}/basic/search?query={query}`
- **Response shape:** `[{sourceName: string, sourceLink: string, text: string}]`
- **Validation:** query required, non-empty, meaningful input
- **Playwright tests:** Tank writes these after the app is built

## Learnings

### Vue app build — 2025

**Files created under `src/vue-app/`:**
- `package.json`, `vite.config.ts`, `tsconfig.json`, `tsconfig.node.json`, `index.html`
- `postcss.config.js`, `tailwind.config.js`, `.env`, `.env.example`, `env.d.ts`
- `src/main.ts`, `src/App.vue`, `src/assets/main.css`
- `src/types/search.ts`
- `src/services/searchService.ts`
- `src/composables/useSearch.ts`
- `src/router/index.ts`
- `src/views/SearchView.vue`
- `src/components/AppHeader.vue`, `SearchInput.vue`, `SearchResults.vue`, `ResultCard.vue`

**Deviations from spec:**
- Added `hasSearched.value = true` in the `catch` block of `useSearch.ts` so the error state renders correctly after a failed request (spec omitted this, but it's required for `SearchResults` to show the error state).
- `applyHint` uses `void handleSearch(hint)` to satisfy `noUnusedLocals` + floating-promise rules from strict TS.
- `SearchInput.vue` uses Unicode escape `\u2026` for ellipsis in the button label to keep the file clean.

**npm install commands that worked:**
```
cd C:\Work\Projects\azure-demos-agents-from-scratch-to-enterprise\src\vue-app
npm install
npx vite build   # ✓ built in ~5s, 108 kB JS, 15 kB CSS
```
