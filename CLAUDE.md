# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

FieldOps.Api is a .NET 9 Web API project that demonstrates modern ASP.NET Core error handling with built-in ProblemDetails support. The solution consists of a single web API project focused on consistent error responses and exception mapping.

## Architecture

- **Framework**: .NET 9 Web API with minimal APIs
- **Error Handling**: Built-in ProblemDetails with custom exception mapping using IExceptionHandler
- **Structure**: Single project solution with Program.cs containing all configuration
- **Exception Mapping**: Custom handler maps InvalidOperationException to 409 Conflict responses
- **Tracing**: All error responses include traceId for debugging

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

### Testing
```bash
# Run tests (if test projects exist)
dotnet test
```

### Development URLs
- HTTP: http://localhost:5090
- HTTPS: https://localhost:7149

### Available Endpoints
- `/ping` - Health check with runtime info
- `/healthz` - Simple health status
- `/boom` - Demo unhandled exception (returns 500)
- `/conflict` - Demo business rule conflict (returns 409)

## Project Structure

- `Program.cs` - Main application entry point with all service configuration and endpoint definitions
- `appsettings.json` / `appsettings.Development.json` - Configuration files
- `Properties/launchSettings.json` - Development server configuration

## Key Features

The application demonstrates:
- Consistent ProblemDetails JSON responses for all errors
- Custom exception mapping (InvalidOperationException → 409 Conflict)
- Trace ID injection for debugging
- Single error handling pipeline for all environments