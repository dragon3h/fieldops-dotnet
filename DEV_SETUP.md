# FieldOps.Api - Development Setup Guide

This guide provides step-by-step instructions for setting up and running the FieldOps.Api project in development mode.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Project Overview](#project-overview)
- [Quick Start](#quick-start)
- [Detailed Setup](#detailed-setup)
- [Available Endpoints](#available-endpoints)
- [Troubleshooting](#troubleshooting)
- [Development Workflows](#development-workflows)

## Prerequisites

Before you begin, ensure you have the following installed:

- **.NET 9 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Docker Desktop** - [Download here](https://www.docker.com/products/docker-desktop)
- **Git** - [Download here](https://git-scm.com/downloads)
- **Visual Studio Code or Visual Studio 2022** (optional but recommended)

Verify your installation:

```bash
dotnet --version  # Should be 9.x.x
docker --version  # Should be 20.x.x or higher
```

## Project Overview

FieldOps.Api is a .NET 9 Web API solution with Clean Architecture principles:

```
fieldops-dotnet/
├── FieldOps.Api/             # Web API layer (controllers, minimal APIs)
├── FieldOps.Application/     # Application layer (interfaces)
├── FieldOps.Domain/          # Domain layer (entities, models)
└── FieldOps.Infrastructure/  # Infrastructure layer (EF Core, repositories)
```

**Key Technologies:**
- ASP.NET Core 9 Web API
- Entity Framework Core 9
- PostgreSQL 17
- Docker & Docker Compose
- OpenAPI/Swagger with Scalar UI

## Quick Start

### Option A: Quick Start (Local .NET + Docker Database)

Get the application running in 5 minutes:

```bash
# 1. Clone the repository (if not already done)
git clone <repository-url>
cd fieldops-dotnet

# 2. Start PostgreSQL database only
cd FieldOps.Infrastructure
docker compose up -d
cd ..

# 3. Apply database migrations
dotnet ef database update --project FieldOps.Infrastructure --startup-project FieldOps.Api

# 4. Run the application
dotnet run --project FieldOps.Api

# 5. Open your browser
# Navigate to https://localhost:7149/scalar
```

### Option B: Quick Start (Full Docker Compose)

Run the entire application stack in Docker:

```bash
# 1. Clone the repository
git clone <repository-url>
cd fieldops-dotnet

# 2. Start the entire stack (API + Database + pgAdmin)
docker compose up -d

# 3. Open your browser
# Navigate to http://localhost:5090/scalar
```

That's it! The API is now running with automatic migrations applied on startup.

## Detailed Setup

### Step 1: Clone and Build

```bash
# Clone the repository
git clone <repository-url>
cd fieldops-dotnet

# Restore dependencies and build
dotnet restore
dotnet build
```

### Step 2: Start Database Services

The project uses Docker Compose to run PostgreSQL and pgAdmin:

```bash
# Navigate to Infrastructure folder
cd FieldOps.Infrastructure

# Start containers in detached mode
docker compose up -d

# Verify containers are running
docker compose ps
```

**Database Connection Details:**
- **PostgreSQL Host:** localhost
- **Port:** 5433 (mapped from container port 5432)
- **Database:** fieldops_dev
- **Username:** postgres
- **Password:** postgres

**pgAdmin Access:**
- **URL:** http://localhost:5050
- **Email:** admin@admin.com
- **Password:** admin

### Step 3: Apply Database Migrations

Entity Framework Core migrations create the database schema:

```bash
# From the solution root directory
dotnet ef database update --project FieldOps.Infrastructure --startup-project FieldOps.Api
```

This command will:
1. Connect to PostgreSQL on localhost:5433
2. Create the `fieldops_dev` database (if it doesn't exist)
3. Apply all migrations from `FieldOps.Infrastructure/Migrations/`

**Verify the migration:**

```bash
# List applied migrations
dotnet ef migrations list --project FieldOps.Infrastructure --startup-project FieldOps.Api
```

### Step 4: Run the Application

There are two launch profiles available:

**Option A: HTTPS (Recommended)**
```bash
dotnet run --project FieldOps.Api --launch-profile https
```
- API runs on https://localhost:7149 and http://localhost:5090
- Swagger UI available at https://localhost:7149/scalar

**Option B: HTTP Only**
```bash
dotnet run --project FieldOps.Api --launch-profile http
```
- API runs on http://localhost:5090 only

**Option C: Default Profile**
```bash
dotnet run --project FieldOps.Api
```
- Uses the first profile in launchSettings.json (http)

### Step 5: Verify Everything Works

Test the API endpoints:

```bash
# Health check
curl http://localhost:5090/ping

# Liveness probe
curl http://localhost:5090/health

# Readiness probe
curl http://localhost:5090/ready

# Get all bounce castles
curl http://localhost:5090/api/v1/bouncycastle
```

**Open Swagger UI:**
- Navigate to https://localhost:7149/scalar (HTTPS profile)
- Or http://localhost:5090/scalar (HTTP profile)

## Available Endpoints

### Health & Status Endpoints

| Method | Endpoint   | Description                              |
|--------|-----------|------------------------------------------|
| GET    | /ping     | Basic health check with runtime info     |
| GET    | /health   | Liveness probe (Kubernetes ready)        |
| GET    | /ready    | Readiness probe with detailed JSON       |
| GET    | /boom     | Test unhandled exception (500 response)  |
| GET    | /conflict | Test business rule conflict (409 response)|

### BouncyCastle API (REST)

| Method | Endpoint                      | Description                |
|--------|------------------------------|----------------------------|
| GET    | /api/v1/bouncycastle         | Get all bounce castles     |
| GET    | /api/v1/bouncycastle/{id}    | Get bounce castle by ID    |
| POST   | /api/v1/bouncycastle         | Create new bounce castle   |
| PUT    | /api/v1/bouncycastle/{id}    | Update bounce castle       |
| DELETE | /api/v1/bouncycastle/{id}    | Delete bounce castle       |

### OpenAPI Documentation

| Method | Endpoint                    | Description                |
|--------|----------------------------|----------------------------|
| GET    | /scalar                    | Scalar API documentation UI |
| GET    | /openapi/v1.json           | OpenAPI specification JSON  |

**Note:** OpenAPI endpoints are only available in Development mode.

## Docker Development

The project includes comprehensive Docker support for both local development and production deployments.

### Docker Compose Files

The repository contains three Docker Compose configurations:

1. **`docker-compose.yml` (Root)** - Production-ready full stack
   - API container with multi-stage build
   - PostgreSQL database with health checks
   - pgAdmin for database management
   - Auto-applies migrations on startup

2. **`docker-compose.override.yml`** - Development overrides
   - Automatically merged with `docker-compose.yml` in development
   - Configures development environment variables
   - Enables verbose logging and debugging

3. **`FieldOps.Infrastructure/docker-compose.yml`** - Database only
   - Standalone PostgreSQL and pgAdmin setup
   - Use when running API locally via `dotnet run`

### Running with Docker Compose

**Full Stack (Recommended for Testing):**
```bash
# Start all services (API + Database + pgAdmin)
docker compose up -d

# View logs
docker compose logs -f

# View API logs only
docker compose logs -f api

# Stop all services
docker compose down

# Stop and remove volumes (clean slate)
docker compose down -v
```

**Database Only (For Local Development):**
```bash
# Navigate to Infrastructure folder
cd FieldOps.Infrastructure

# Start database and pgAdmin only
docker compose up -d

# Return to root
cd ..

# Run API locally
dotnet run --project FieldOps.Api
```

### Docker Build Targets

The `FieldOps.Api/Dockerfile` includes multiple build targets:

- **`base`** - Runtime base image (for Visual Studio debugging)
- **`build`** - Build stage with SDK
- **`publish`** - Release build output
- **`final`** - Production runtime (default)
- **`dev`** - Development mode with `dotnet watch` and remote debugging

**Build for production:**
```bash
docker build -t fieldops-api:latest -f FieldOps.Api/Dockerfile .
```

**Build for development:**
```bash
docker build -t fieldops-api:dev --target dev -f FieldOps.Api/Dockerfile .
```

### Environment Variables

When running in Docker, the following environment variables are available:

| Variable | Default | Description |
|----------|---------|-------------|
| `ASPNETCORE_ENVIRONMENT` | Production | Environment name (Development/Staging/Production) |
| `ASPNETCORE_URLS` | http://+:5090 | URLs to listen on |
| `ConnectionStrings__DefaultConnection` | (see docker-compose.yml) | PostgreSQL connection string |
| `MIGRATE_ON_STARTUP` | true | Auto-apply EF migrations on startup |

**Override in docker-compose.override.yml:**
```yaml
services:
  api:
    environment:
      MIGRATE_ON_STARTUP: "false"
      ASPNETCORE_ENVIRONMENT: "Development"
```

### Migration-on-Startup

The API automatically applies EF Core migrations when running in Docker:

- Enabled via `MIGRATE_ON_STARTUP=true` environment variable
- Retries up to 12 times (60 seconds total) waiting for database
- Logs each attempt and shows applied/pending migrations
- Throws exception if migrations fail after all retries

**Disable auto-migration:**
```bash
# Set environment variable
export MIGRATE_ON_STARTUP=false
docker compose up
```

### Visual Studio Docker Support

The solution includes Visual Studio Docker launch profiles:

**Launch with Docker:**
1. Open `fieldops-dotnet.sln` in Visual Studio 2022
2. Set launch profile to "Docker" or "Docker Compose"
3. Press F5 to debug

Visual Studio will:
- Build the Docker image
- Start containers
- Attach the debugger
- Open browser to http://localhost:5090/scalar

### Development with dotnet watch in Docker

For hot-reload development in Docker:

```bash
# Build dev image
docker build -t fieldops-api:dev --target dev -f FieldOps.Api/Dockerfile .

# Run with source code mounted
docker run -it --rm \
  -p 5090:5090 \
  -v $(pwd):/src \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5433;Database=fieldops_dev;Username=postgres;Password=postgres" \
  fieldops-api:dev
```

Code changes will automatically trigger recompilation.

### Docker Networking

When running services in Docker Compose:

- **API → Database:** Use hostname `db` (internal Docker network)
- **Host → API:** Use `localhost:5090` (port forwarding)
- **Host → Database:** Use `localhost:5433` (port forwarding)
- **pgAdmin → Database:** Use hostname `db` and port `5432` (internal)

### Health Checks

The database includes a health check in docker-compose.yml:

```yaml
db:
  healthcheck:
    test: ["CMD-SHELL", "pg_isready -U postgres"]
    interval: 10s
    timeout: 5s
    retries: 5
```

The API waits for the database to be healthy before starting:

```yaml
api:
  depends_on:
    db:
      condition: service_healthy
```

## Troubleshooting

### Database Connection Issues

**Problem:** `Connection refused` or `cannot connect to database`

**Solution:**
```bash
# Check if PostgreSQL container is running
docker compose ps

# View container logs
docker compose logs postgres

# Restart containers
docker compose down
docker compose up -d

# Verify port 5433 is not in use
lsof -i :5433  # macOS/Linux
netstat -ano | findstr :5433  # Windows
```

### Migration Errors

**Problem:** `A connection was successfully established... but then closed`

**Solution:**
```bash
# Ensure database container is fully started (check health)
docker compose ps

# Drop and recreate database
dotnet ef database drop --project FieldOps.Infrastructure --startup-project FieldOps.Api --force
dotnet ef database update --project FieldOps.Infrastructure --startup-project FieldOps.Api
```

### Port Conflicts

**Problem:** `Address already in use: 5090` or `7149`

**Solution:**
```bash
# Find process using the port (macOS/Linux)
lsof -i :5090
kill -9 <PID>

# Find process using the port (Windows)
netstat -ano | findstr :5090
taskkill /PID <PID> /F

# Or change ports in launchSettings.json
```

### Docker Compose Issues

**Problem:** Docker containers won't start

**Solution:**
```bash
# Remove existing containers and volumes
docker compose down -v

# Pull fresh images
docker compose pull

# Start with verbose logging
docker compose up
```

### Entity Framework Tools Not Found

**Problem:** `dotnet ef` command not found

**Solution:**
```bash
# Install EF Core tools globally
dotnet tool install --global dotnet-ef

# Update existing tools
dotnet tool update --global dotnet-ef

# Verify installation
dotnet ef --version
```

# Add EF tools to a project if you need Package Manager commands
Install-Package Microsoft.EntityFrameworkCore.Tools -ProjectName FieldOps.Api

# Then run the EF PMC command (from PM Console)
Update-Database -Project FieldOps.Infrastructure -StartupProject FieldOps.Api
```

## Development Workflows

### Creating a New Migration

When you modify entity models:

```bash
# Create migration
dotnet ef migrations add <MigrationName> --project FieldOps.Infrastructure --startup-project FieldOps.Api

# Review generated migration in FieldOps.Infrastructure/Migrations/

# Apply migration
dotnet ef database update --project FieldOps.Infrastructure --startup-project FieldOps.Api
```

### Hot Reload

The project supports .NET Hot Reload for rapid development:

```bash
# Run with hot reload enabled (default)
dotnet watch --project FieldOps.Api
```

Changes to .cs files will be automatically applied without restarting the app.

### Viewing Database with pgAdmin

1. Open http://localhost:5050
2. Login with admin@admin.com / admin
3. Right-click "Servers" → "Register" → "Server"
4. **General tab:** Name = "FieldOps Dev"
5. **Connection tab:**
   - Host: postgres (container name) or host.docker.internal
   - Port: 5432 (internal container port)
   - Database: fieldops_dev
   - Username: postgres
   - Password: postgres
6. Click "Save"

### Testing API Endpoints

**Using curl:**
```bash
# Create a bounce castle
curl -X POST http://localhost:5090/api/v1/bouncycastle \
  -H "Content-Type: application/json" \
  -d '{"name":"Super Jumper","capacity":20,"pricePerDay":150.00,"isAvailable":true}'

# Get all bounce castles
curl http://localhost:5090/api/v1/bouncycastle

# Get specific bounce castle
curl http://localhost:5090/api/v1/bouncycastle/1

# Update bounce castle
curl -X PUT http://localhost:5090/api/v1/bouncycastle/1 \
  -H "Content-Type: application/json" \
  -d '{"id":1,"name":"Mega Jumper","capacity":25,"pricePerDay":175.00,"isAvailable":true}'

# Delete bounce castle
curl -X DELETE http://localhost:5090/api/v1/bouncycastle/1
```

**Using Scalar UI:**
1. Navigate to https://localhost:7149/scalar
2. Explore all endpoints with interactive documentation
3. Test requests directly from the UI

### Stopping Development Environment

```bash
# Stop the API (Ctrl+C in terminal)

# Stop database containers
cd FieldOps.Infrastructure
docker compose down

# Stop and remove volumes (clean slate)
docker compose down -v
```

### Environment Configuration

The application uses standard ASP.NET Core configuration:

- **appsettings.json** - Production settings
- **appsettings.Development.json** - Development overrides
- **launchSettings.json** - Launch profiles and environment variables

**Connection string location:**
[appsettings.Development.json](FieldOps.Api/appsettings.Development.json#L2-L4)

**CORS configuration:**
[Program.cs](FieldOps.Api/Program.cs#L34-L41) - Allows requests from `http://localhost:3000` and `http://localhost:5173`

## Additional Resources

- **.NET 9 Documentation:** https://learn.microsoft.com/dotnet/core/whats-new/dotnet-9/overview
- **EF Core 9 Documentation:** https://learn.microsoft.com/ef/core/what-is-new/ef-core-9.0/whatsnew
- **PostgreSQL Documentation:** https://www.postgresql.org/docs/17/
- **Docker Compose Documentation:** https://docs.docker.com/compose/
- **Repository Guidelines:** See [AGENTS.md](AGENTS.md) for contribution standards and workflows.

## Next Steps

- Add authentication/authorization
- Implement unit and integration tests
- Set up CI/CD pipeline
- Add logging and monitoring
- Configure production database settings
- Implement caching layer
- Add API rate limiting

---

**Need help?** Check the [CLAUDE.md](CLAUDE.md) file for project-specific commands and architecture details.
