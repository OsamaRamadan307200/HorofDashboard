# Horof Dashboard

An ASP.NET Core 8.0 MVC admin dashboard for managing content in the Horof Arabic letter-learning app (Levels → Units → Lessons → Slides hierarchy).

## Run & Operate

- Workflow: `artifacts/horof-dashboard: web` — runs `dotnet run --project /home/runner/workspace/dashboard/HorofDashboard.csproj` on port 18288
- `pnpm --filter @workspace/api-server run dev` — run the Node.js API server (port 8080, unused by dashboard)
- `pnpm run typecheck` — full typecheck across Node packages
- Required env (optional override): `MSSQL_CONNECTION_STRING` — SQL Server connection string (falls back to appsettings.json value)

## Stack

- ASP.NET Core 8.0 MVC (C#), Entity Framework Core, SQL Server
- Frontend: Bootstrap 5, Font Awesome 6.5, custom CSS variables
- Dark sidebar layout with collapsible nav (localStorage state)
- pnpm workspaces (Node.js side — API server, mockup sandbox)

## Where things live

- `dashboard/` — the entire .NET MVC project
- `dashboard/Controllers/` — MVC controllers (Levels, Units, Lessons, Slides, Home)
- `dashboard/Views/` — Razor views per controller + Shared/_Layout.cshtml
- `dashboard/Models/` — EF Core models + ViewModels
- `dashboard/wwwroot/css/site.css` — all custom styles (CSS variables, sidebar, cards)
- `dashboard/appsettings.json` — SQL Server connection string (production DB)
- `artifacts/horof-dashboard/` — artifact registration only (artifact.toml, no source)

## Architecture decisions

- No authentication — admin-only internal tool
- SQL Server hosted externally at SQL1004.site4now.net (credentials in appsettings.json, already public on GitHub)
- `MSSQL_CONNECTION_STRING` env var used instead of `DATABASE_URL` to avoid collision with Replit's PostgreSQL env var
- `UseHttpsRedirection()` removed — Replit proxy handles HTTPS termination
- `LessonsCount` on Unit is auto-synced on every lesson create/edit/delete

## Product

- Dashboard home: live counts of Levels, Units, Lessons, Slides with quick-add buttons
- CRUD for all 4 content types with full Arabic text support
- Slide types: MultipleChoice, TrueFalse, VoiceChoices, Completion, Ordering
- Unit delete is guarded — blocks deletion if lessons exist

## User preferences

- Keep .NET MVC stack (no migration to other frameworks)
- No authentication required

## Gotchas

- Do NOT rename `DATABASE_URL` — use `MSSQL_CONNECTION_STRING` for the SQL Server override to avoid conflict with Replit's built-in PostgreSQL env
- `dotnet run` compiles on first start — allow ~15-20s for the workflow to become ready
- The `artifacts/horof-dashboard/` directory is just an artifact stub; all real source is in `dashboard/`
