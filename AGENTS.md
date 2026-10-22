# AGENTS.md

Personal finance tracker ("finn"), single-user, all strings/comments/UI in Portuguese (pt-BR). Two apps served as one service: a .NET 8 API in `api/` that also serves the built React SPA, and the React/Vite frontend in `app/`. No test suite exists; verification = `dotnet build` + `npm run lint` + `npm run build`.

## Commands

API (run from `api/` — required, see DB path below):
```powershell
$env:ASPNETCORE_ENVIRONMENT="Development"; $env:PORT="5021"; $env:WEB_URL="http://localhost:3000"; dotnet run
```
- `PORT` sets the listen URL (`Program.cs` forces `http://0.0.0.0:$PORT`); `WEB_URL` is the CORS allowed origin. Without them the app fails at startup.
- `LoadEnv.Load()` (reads `api/.env`) is commented out in `Program.cs` — env templates live in `api/.env.exemple` and root `.env.exemple`. Required vars: `PORT`, `WEB_URL`, `B3_API_URL`/`B3_API_KEY` (brapi.dev quotes), `CRYPTO_API_URL`/`CRYPTO_API_KEY` (coingecko).

Frontend (from `app/`):
```powershell
npm run dev    # vite, port 3000
npm run lint
npm run build  # tsc -b && vite build
```
- Vite has NO dev proxy; axios hits `VITE_API_BASE_URL` or falls back to `http://localhost:5000/api` (`app/src/services/api.ts`). If the API listens on a different port locally, export `VITE_API_BASE_URL` or override the fallback target.

Migrations (EF Core + SQLite):
```powershell
cd api; dotnet ef migrations add <Name>; dotnet run -- --migrate
```
- `--migrate` is a CLI mode in `Program.cs` that applies migrations and exits; in K8s it runs as the init container, so don't treat it as app startup.

## Architecture

- Single .NET project `api/api.csproj` (no solution structure to respect; `api.sln` just wraps it). Vertical slices under `api/src/core/modules/{Aportes,Movimentacoes,Saldos,Salarios}/` each with `controllers/dtos/models/repositories/useCases`. Shared entities (e.g. `Rotina`) in `api/src/core/entities/`; cross-cutting (`Context`, middlewares, `LoadEnv`, `BusinessError`) in `api/src/infra/`.
- Every new controller, repository, and use case must be manually registered in `Program.cs` DI.
- DB path is hardcoded relative: `Data Source=data/finndb.sqlite` (`api/src/infra/database/Context.cs`). Running dotnet outside `api/` as cwd creates a wrong DB elsewhere.
- `Program.cs` pins `CultureInfo.DefaultThreadCurrentCulture = InvariantCulture` — do not remove; decimal parsing across the app depends on it.
- `Context.OnModelCreating` seeds categories and two Saldos via `HasData` — changing seeds means a new migration.
- Codenames are domain Portuguese: Movimentacoes = expenses/transactions, Aportes = investments, Saldos = balances, Salarios = salary.
- Frontend POST requests pass string values; a request interceptor (`convertStringsToNumbers`) coerces them to numbers — DTO fields are numeric despite string inputs (`app/src/services/api.ts`).

## Production / deploy

- In prod one container serves everything: Dockerfile builds `app/` (copying `yarn*.lock`, `npm i`, then `yarn build --debug` — yes, mixed npm/yarn; both lockfiles exist) and copies `app/dist` into the API's `wwwroot`. All API routes live under `/api`; SPA fallback via `MapFallbackToFile("index.html")`.
- `VITE_API_BASE_URL` is baked in as a Docker build-arg (`API_BASE_URL`) — changing the frontend API URL requires rebuilding the image.
- Push to `main` triggers a self-hosted GitHub Actions runner: docker build + release. Releases come from `conventional-changelog-action`, so **always use conventional commits** (`feat:`, `fix:`, `chore:`) on main.
- `deploy/deploy.yaml` is the microk8s manifest (image from local registry `localhost:32000`, port 30001); migrations run via the init container described above.
