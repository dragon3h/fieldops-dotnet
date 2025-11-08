using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Scalar.AspNetCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using FieldOps.Infrastructure;
using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Infrastructure.Data;
using FieldOps.Infrastructure.Services.Repositories;
using BouncyCastleEntity = FieldOps.Domain.BouncyCastle.BouncyCastle;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register endpoints API explorer and OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
// 1) Services — register health checks with tags
builder.Services
    .AddHealthChecks()
    // Liveness: trivial "self" check (no external deps)
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "live" })
    // Readiness: start with a placeholder check; we'll add Postgres/Redis later
    .AddCheck("startup-ready", () => HealthCheckResult.Healthy("App bootstrapped."), tags: new[] { "ready" });

var connectionString = builder.Configuration.GetConnectionString("BouncyCastleDatabase")
    ?? throw new InvalidOperationException("Connection string 'BouncyCastleDatabase' not found.");
// Use the DbContext from FieldOps.Infrastructure (no local definition here)
builder.Services.AddDbContext<FieldOpsDbContext>(options =>
  options.UseNpgsql(connectionString));
builder.Services.AddScoped<IRepository<BouncyCastleEntity>, BouncyCastleRepository>();


// Services
builder.Services.AddCors(options =>
{
  options.AddPolicy("Dev", policy => policy
      .WithOrigins("http://localhost:3000", "http://localhost:5173")
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowCredentials());
});


// 1) ProblemDetails with traceId on every payload
builder.Services.AddProblemDetails(options =>
{
  options.CustomizeProblemDetails = ctx =>
  {
    var traceId = Activity.Current?.Id ?? ctx.HttpContext.TraceIdentifier;
    ctx.ProblemDetails.Extensions["traceId"] = traceId;
    ctx.ProblemDetails.Instance ??= ctx.HttpContext.Request.Path;
    ctx.ProblemDetails.Type ??= "https://tools.ietf.org/html/rfc9110#section-15.6.1";
  };
});

// 2) Map InvalidOperationException -> 409 via IExceptionHandler
builder.Services.AddExceptionHandler<InvalidOperationToConflictHandler>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

// 3) One consistent error pipeline (all environments)
app.UseExceptionHandler();   // unhandled + mapped exceptions -> ProblemDetails JSON
app.UseStatusCodePages();    // 404/405/etc -> ProblemDetails JSON
app.UseCors("Dev");          // CORS errors -> ProblemDetails JSON

// Liveness: fast and always healthy unless process is failing
app.MapHealthChecks("/health", new HealthCheckOptions
{
  Predicate = r => r.Tags.Contains("live")
});

// Readiness: run only checks tagged "ready" and return JSON
app.MapHealthChecks("/ready", new HealthCheckOptions
{
  Predicate = r => r.Tags.Contains("ready"),
  ResponseWriter = async (context, report) =>
  {
    context.Response.ContentType = "application/json";
    var result = System.Text.Json.JsonSerializer.Serialize(new
    {
      status = report.Status.ToString(),
      checks = report.Entries.Select(e => new
      {
        name = e.Key,
        status = e.Value.Status.ToString(),
        description = e.Value.Description
      })
    });
    await context.Response.WriteAsync(result);
  }
});

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();           // /openapi/v1.json
  app.MapScalarApiReference(); // /scalar
}

// Demo endpoints
app.MapGet("/ping", () => Results.Ok(new
{
  ok = true,
  runtime = Environment.Version.ToString(),
  app = "FieldOps.Api"
}));

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

// Unhandled exception -> 500
app.MapGet("/boom", (HttpContext _) =>
    Task.FromException(new Exception("Sample unhandled exception.")));

// Mapped InvalidOperationException -> 409 (handled by IExceptionHandler)
app.MapGet("/conflict", (HttpContext _) =>
    Task.FromException(new InvalidOperationException("Business rule conflict.")));


app.Run();

// ---- local class for exception mapping ----
sealed class InvalidOperationToConflictHandler : IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
  {
    if (exception is not InvalidOperationException) return false;

    var problemDetailsService = httpContext.RequestServices
        .GetRequiredService<IProblemDetailsService>();

    httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

    await problemDetailsService.WriteAsync(new ProblemDetailsContext
    {
      HttpContext = httpContext,
      ProblemDetails = new ProblemDetails
      {
        Title = "Conflict",
        Status = StatusCodes.Status409Conflict,
        Detail = exception.Message
      }
    });

    return true; // handled
  }
}
