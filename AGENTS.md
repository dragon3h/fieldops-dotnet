# Repository Guidelines

## Project Structure & Module Organization
- `FieldOps.Api/` hosts the ASP.NET Core entry point, controllers, and configuration. The project file references the other layers; keep HTTP-only code here.
- `FieldOps.Application/` defines service contracts and interface abstractions consumed by the API. Place orchestration logic and cross-layer DTOs here.
- `FieldOps.Domain/` contains entities, aggregates, and value objects (e.g., `Order/`, `Payment/`). Keep domain logic self-contained and persistence-agnostic.
- `FieldOps.Infrastructure/` implements EF Core persistence, migrations, and the Docker compose tooling for PostgreSQL. Align repository implementations with interfaces and keep connection strings in `appsettings.Development.json`.

## Build, Test, and Development Commands
- `dotnet restore` then `dotnet build` at the repository root restore packages and compile the entire solution.
- `dotnet run --project FieldOps.Api` starts the API; prefer `dotnet watch --project FieldOps.Api` to enable hot reload while iterating.
- `docker compose up -d` from `FieldOps.Infrastructure/` starts PostgreSQL and pgAdmin; `docker compose down -v` resets them.
- `dotnet ef database update --project FieldOps.Infrastructure --startup-project FieldOps.Api` applies migrations. Regenerate scaffolding with `dotnet ef migrations add <Name> --project FieldOps.Infrastructure --startup-project FieldOps.Api`.

## Coding Style & Naming Conventions
- Use C# 12 features with two-space indentation, file-scoped namespaces, and top-level programs as already adopted in `Program.cs`.
- Keep classes, interfaces, and records in PascalCase; async methods end with `Async`; private fields use `_camelCase`.
- Run `dotnet format` before submitting to enforce whitespace, ordering, and nullable annotations.

## Testing Guidelines
- Add new test projects under `tests/` following the solution naming pattern (e.g., `tests/FieldOps.Api.Tests`). Register them in `fieldops-dotnet.sln`.
- Prefer xUnit with fluent assertions; name test files `<Subject>Tests.cs` and methods `MethodName_State_ExpectedBehavior`.
- Execute `dotnet test` from the repository root. Aim to cover new endpoints, repositories, and domain rules with integration or unit tests hitting the in-memory provider.

## Commit & Pull Request Guidelines
- Follow the existing history: short, imperative summaries such as "Add infrastructure layer..." and reference the impacted slice first.
- Squash work-in-progress locally; each PR should include description, testing evidence (`dotnet test` output or reasoning), migration callouts, and linked issue or ticket.
- Include screenshots or curl samples for API-facing changes, especially when modifying Scalar or response contracts.

## Security & Configuration Tips
- Do not commit secrets; local overrides belong in `appsettings.Development.json` or user secrets. Keep production values in deploy-time configuration.
- When editing migrations, ensure the Docker database is running and verify schema drift with `dotnet ef migrations script` before deployment.
