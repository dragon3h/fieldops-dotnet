# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

FieldOps.Api is a .NET 9 Web API demonstrating Clean Architecture with EF Core + PostgreSQL, repository/unit-of-work patterns, AutoMapper, and ProblemDetails error handling. It models a bounce castle rental business.

## Architecture

The solution follows Clean Architecture with 5 projects:

```
FieldOps.Api (Presentation)
  └── FieldOps.Application (Use Cases / Interfaces)
        └── FieldOps.Domain (Pure Domain — no external dependencies)
FieldOps.Infrastructure (Data Access — depends on Application + Domain)
FileOps.Tests (XUnit — references FieldOps.Api transitively)
```

**Key patterns:**
- **Repository + Unit of Work**: `IRepository<T>` / `GenericRepository<T>` for CRUD; `IUnitOfWork` wraps `SaveChangesAsync()` for atomic operations. Specialized repositories (`IBouncyCastleRepository`, `IClientRepository`) extend the generic interface for future specialty queries.
- **Service Layer**: `IBouncyCastleService`, `IClientService` handle business logic (GUID generation, audit metadata). Services depend on `IUnitOfWork`.
- **AutoMapper**: Profiles in `FieldOps.Application`. DTO→Domain mappings ignore `Id` and all audit fields. Community license configured.
- **DI Registration**: Centralized in `FieldOps.Infrastructure/DependencyInjection.cs` (`AddInfrastructureServices`) and `FieldOps.Application/ApplicationServiceExtensions.cs` (`AddApplicationServices`). All repositories and services are scoped.

**Domain model highlights:**
- `Product` (abstract) is the base for `BouncyCastle` using **TPC (Table Per Concrete Type)** — `BouncyCastle` gets its own table with all inherited columns.
- `Size` and `Address` are value object records stored as **EF Core owned entities** (columns embedded in parent table).
- All entities implementing `ITracker` carry audit fields (`CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy`); services populate these (currently hardcoded to `"system"`).
- `Guid Id` is `ValueGeneratedNever()` — services generate IDs in `CreateAsync()`.

**Error handling:**
- `InvalidOperationException` is mapped to HTTP 409 Conflict via a sealed `IExceptionHandler` implementation in `Program.cs`.
- All ProblemDetails responses include a `traceId` field injected via the ProblemDetails customizer.

## Common Commands

### Build and Run
```bash
dotnet build
dotnet run --project src/FieldOps.Api
dotnet run --project src/FieldOps.Api --launch-profile https
```

### Database
```bash
# Start PostgreSQL + pgAdmin (development)
cd src/FieldOps.Infrastructure && docker compose up -d

# New migration
dotnet ef migrations add <Name> --project src/FieldOps.Infrastructure --startup-project src/FieldOps.Api

# Apply migrations
dotnet ef database update --project src/FieldOps.Infrastructure --startup-project src/FieldOps.Api
```

### Testing
```bash
# Run all tests
dotnet test

# Run a single test by name
dotnet test --filter "FullyQualifiedName~<TestMethodName>"
```

### Environment Variables
- `MIGRATE_ON_STARTUP=true` — runs EF migrations on startup (used in Docker)
- `ConnectionStrings__DefaultConnection` — overrides appsettings connection string

## Development URLs
- HTTP: http://localhost:5090 | HTTPS: https://localhost:7149
- Scalar UI: https://localhost:7149/scalar (dev only)
- OpenAPI JSON: https://localhost:7149/openapi/v1.json (dev only)
- PostgreSQL: localhost:5433 (postgres/postgres, database: fieldops_dev)
- pgAdmin: http://localhost:5050 (admin@admin.com / admin)

## Available Endpoints

**Minimal API (Program.cs):** `GET /ping`, `GET /health`, `GET /ready`, `GET /boom` (demo 500), `GET /conflict` (demo 409)

**REST Controllers:**
- `api/v1/bouncycastle` — GET, GET/{id}, POST, PUT/{id}, DELETE/{id}
- `api/v1/client` — GET, GET/{id}, POST, PUT/{id}, DELETE/{id}

## Adding New Entities

The pattern to follow when adding a new domain entity:
1. **Domain**: Add entity under `src/FieldOps.Domain/<Entity>/`, inherit from `BaseEntity` + implement `ITracker`. For product types, inherit from `Product`.
2. **Application**: Add `I<Entity>Repository`, `I<Entity>Service`, `<Entity>DTO`, and an AutoMapper profile.
3. **Infrastructure**: Add `<Entity>Repository : GenericRepository<T>, I<Entity>Repository`. Register in `DependencyInjection.cs`. Add `DbSet<T>` to `FieldOpsDbContext` and configure in `OnModelCreating`.
4. **Application layer**: Add `<Entity>Service : I<Entity>Service`. Register in `ApplicationServiceExtensions.cs`.
5. **Api**: Add controller, create migration.
