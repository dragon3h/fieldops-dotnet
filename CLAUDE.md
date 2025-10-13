# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

FieldOps.Api is a .NET 9 Web API solution demonstrating modern ASP.NET Core patterns including error handling with ProblemDetails, Entity Framework Core with PostgreSQL, repository pattern, and RESTful API design. The solution includes a Web API project and an Infrastructure project for data access.

## Architecture

- **Framework**: .NET 9 Web API with both minimal APIs and controllers
- **Database**: PostgreSQL 17 via Entity Framework Core 9 with Npgsql provider
- **Data Access**: Repository pattern with generic `IRepository<T>` interface
- **Error Handling**: Built-in ProblemDetails with custom exception mapping using IExceptionHandler
- **Structure**: Multi-project solution
  - `FieldOps.Api` - Web API layer with controllers and minimal API endpoints
  - `FieldOps.Infrastructure` - Data access layer with EF Core DbContext, repositories, entities, and migrations
- **API Documentation**: OpenAPI/Swagger with Scalar UI (development only)
- **Health Checks**: Liveness (`/health`) and readiness (`/ready`) probes with tagged checks
- **Exception Mapping**: Custom handler maps InvalidOperationException to 409 Conflict responses
- **Tracing**: All error responses include traceId for debugging
- **CORS**: Development CORS policy for local React/Vite apps

## Common Commands

### Build and Run
```bash
# Build the solution
dotnet build

# Run the API locally (uses launchSettings.json profiles)
dotnet run --project FieldOps.Api

# Run with specific launch profile
dotnet run --project FieldOps.Api --launch-profile https
```

### Database
```bash
# Start PostgreSQL and pgAdmin via Docker Compose
cd FieldOps.Infrastructure
docker compose up -d

# Create a new migration
dotnet ef migrations add <MigrationName> --project FieldOps.Infrastructure --startup-project FieldOps.Api

# Apply migrations to database
dotnet ef database update --project FieldOps.Infrastructure --startup-project FieldOps.Api

# Stop Docker containers
docker compose down
```

### Testing
```bash
# Run tests (if test projects exist)
dotnet test
```

### Development URLs
- **API**:
  - HTTP: http://localhost:5090
  - HTTPS: https://localhost:7149
- **Swagger/Scalar UI**: https://localhost:7149/scalar (development only)
- **OpenAPI JSON**: https://localhost:7149/openapi/v1.json (development only)
- **PostgreSQL**: localhost:5433 (user: postgres, password: postgres, database: fieldops_dev)
- **pgAdmin**: http://localhost:5050 (email: admin@admin.com, password: admin)

### Available Endpoints

#### Minimal API Endpoints (Demo)
- `GET /ping` - Health check with runtime info
- `GET /health` - Liveness probe (tagged "live")
- `GET /ready` - Readiness probe with JSON status (tagged "ready")
- `GET /boom` - Demo unhandled exception (returns 500)
- `GET /conflict` - Demo business rule conflict (returns 409)

#### REST API Endpoints (BouncyCastle)
- `GET /api/v1/bouncycastle` - Get all bounce castles
- `GET /api/v1/bouncycastle/{id}` - Get bounce castle by ID
- `POST /api/v1/bouncycastle` - Create a new bounce castle
- `PUT /api/v1/bouncycastle/{id}` - Update an existing bounce castle
- `DELETE /api/v1/bouncycastle/{id}` - Delete a bounce castle

## Project Structure

```
fieldops-dotnet/
├── FieldOps.Api/                    # Web API project
│   ├── Controllers/
│   │   └── BouncyCastleController.cs  # REST API controller for BouncyCastle CRUD
│   ├── Program.cs                      # Application entry point with DI, middleware, and minimal API endpoints
│   ├── appsettings.json                # Production configuration
│   ├── appsettings.Development.json    # Development configuration (connection strings, logging)
│   └── Properties/launchSettings.json  # Development server profiles
│
├── FieldOps.Infrastructure/          # Data access project
│   ├── Entities/
│   │   └── BouncyCastle.cs            # Domain entity with EF Core annotations
│   ├── Migrations/                     # EF Core migration files
│   ├── BouncyCastleDbContext.cs       # EF Core DbContext
│   ├── IRepository.cs                  # Generic repository interface
│   ├── BouncyCastleRepository.cs      # Repository implementation for BouncyCastle
│   └── docker-compose.yml             # PostgreSQL and pgAdmin containers
│
└── fieldops-dotnet.sln               # Solution file
```

## Key Features

The application demonstrates:

### Error Handling
- Consistent ProblemDetails JSON responses for all errors (4xx, 5xx)
- Custom exception mapping (InvalidOperationException → 409 Conflict)
- Trace ID injection for debugging
- Single error handling pipeline for all environments

### Data Access
- Entity Framework Core 9 with PostgreSQL via Npgsql
- Repository pattern with generic `IRepository<T>` interface
- Code-first database with migrations
- Domain entity: `BouncyCastle` (rental equipment tracking)

### API Features
- RESTful API with proper HTTP verbs (GET, POST, PUT, DELETE)
- OpenAPI/Swagger documentation via Scalar UI
- Health checks with liveness and readiness probes
- CORS support for local frontend development
- Consistent API versioning (`/api/v1/...`)

### Development Tools
- Docker Compose for local PostgreSQL database
- pgAdmin for database management UI
- Hot reload support for rapid development