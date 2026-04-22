# Morpheus — History

## Project Context

- **Project:** azure-demos-agents-from-scratch-to-enterprise
- **Backend:** ASE.EnterpriseApi (ASP.NET Core)
- **Search endpoint:** `GET /basic/search?query={query}` → `List<SearchResult>`
- **SearchResult model:** `{ SourceName: string, SourceLink: string, Text: string }`
- **Default URL:** https://localhost:5066
- **User:** Bojan Vrhovnik

## Learnings

### CORS configuration for ASE.EnterpriseApi
- **Policy name:** `"AllowVueApp"`
- **Allowed origins:** `http/https://localhost:5173` (Vite dev), `http/https://localhost:4173` (Vite preview), `http/https://localhost:3000`
- **Middleware pipeline order:** `UseForwardedHeaders` → `UseCors("AllowVueApp")` → `MapGroup(...)` → `UseExceptionHandler` → `MapHealthChecks`
- `UseCors` MUST be placed before route mapping (`MapGroup`) for it to apply to endpoint responses
