# Morpheus — History

## Project Context

- **Project:** azure-demos-agents-from-scratch-to-enterprise
- **Backend:** ASE.EnterpriseApi (ASP.NET Core)
- **Search endpoint:** `GET /basic/search?query={query}` → `List<SearchResult>`
- **SearchResult model:** `{ SourceName: string, SourceLink: string, Text: string }`
- **Default URL:** https://localhost:5066
- **User:** Bojan Vrhovnik

## Learnings

### Dockerfile fix and run scripts (2025-07-17)
- **Dockerfile fix:** Changed `COPY ["ASE.EnterpriseApi/", ...]` (copies entire dir) to `COPY ["ASE.EnterpriseApi/ASE.EnterpriseApi.csproj", ...]` + added `COPY ["ASE.Libraries/ASE.Libraries.csproj", ...]` before `dotnet restore`. Build context must be `AgentScratchEnterprise/` (parent dir).
- **Added WORKDIR /app** to base stage; removed spurious `EXPOSE 8081`.
- **scripts/run-backend.ps1:** Runs `dotnet run` with `ASPNETCORE_ENVIRONMENT=Development` on `http://localhost:5066`. Handles Ctrl+C with try/finally.
- **src/chat-web-app/.dockerignore:** Excludes node_modules, dist, .env files, test artifacts, and Playwright config from Docker build context.

### CORS configuration for ASE.EnterpriseApi
- **Policy name:** `"AllowVueApp"`
- **Allowed origins:** `http/https://localhost:5173` (Vite dev), `http/https://localhost:4173` (Vite preview), `http/https://localhost:3000`
- **Middleware pipeline order:** `UseForwardedHeaders` → `UseCors("AllowVueApp")` → `MapGroup(...)` → `UseExceptionHandler` → `MapHealthChecks`
- `UseCors` MUST be placed before route mapping (`MapGroup`) for it to apply to endpoint responses
