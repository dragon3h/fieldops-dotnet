# Multi-stage Dockerfile for FieldOps.Api
# - base (runtime base for Visual Studio debugging)
# - build (SDK for building)
# - publish (Release publish)
# - final (production runtime)
# - dev (SDK with dotnet watch for development)

# Base stage - used by Visual Studio for fast mode debugging
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5090
ENV ASPNETCORE_URLS="http://+:5090"
ENV ASPNETCORE_ENVIRONMENT=Development
ENV MIGRATE_ON_STARTUP="true"

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj files and restore to leverage Docker layer caching
COPY ["src/FieldOps.Api/FieldOps.Api.csproj", "FieldOps.Api/"]
COPY ["src/FieldOps.Application/FieldOps.Application.csproj", "FieldOps.Application/"]
COPY ["src/FieldOps.Domain/FieldOps.Domain.csproj", "FieldOps.Domain/"]
COPY ["src/FieldOps.Infrastructure/FieldOps.Infrastructure.csproj", "FieldOps.Infrastructure/"]
RUN dotnet restore "FieldOps.Api/FieldOps.Api.csproj"

# Copy source and build
COPY ./src .
WORKDIR "/src/FieldOps.Api"
RUN dotnet build "FieldOps.Api.csproj" -c Debug -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "FieldOps.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage - production runtime
FROM base AS final
WORKDIR /app
ENV MIGRATE_ON_STARTUP="true"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FieldOps.Api.dll"]

# Dev stage for local development (dotnet watch + mounted source + debugging)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
WORKDIR /src

# Install vsdbg (Visual Studio Debugger) for remote debugging
RUN apt-get update && apt-get install -y --no-install-recommends \
    curl unzip procps \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg \
    && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS="http://+:5090;https://+:7149"
# In dev we also enable migration-on-start for convenience; adjust as needed.
ENV MIGRATE_ON_STARTUP="true"
# Disable HTTPS redirect for easier debugging
ENV ASPNETCORE_Kestrel__Endpoints__Http__Url="http://0.0.0.0:5090"

# fallback copy so image can run even without mounts; during dev you will mount the source.
COPY . .
WORKDIR /src/FieldOps.Api
EXPOSE 5090 7149
ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:5090"]