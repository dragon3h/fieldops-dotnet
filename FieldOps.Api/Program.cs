// Program.cs (.NET 9) — Built-in ProblemDetails + custom exception mapping
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// 3) One consistent error pipeline (all environments)
app.UseExceptionHandler();   // unhandled + mapped exceptions -> ProblemDetails JSON
app.UseStatusCodePages();    // 404/405/etc -> ProblemDetails JSON

// Demo endpoints
app.MapGet("/ping", () => Results.Ok(new
{
    ok = true,
    runtime = Environment.Version.ToString(),
    app = "FieldOps.Api"
}));

app.MapGet("/healthz", () => Results.Ok(new { status = "Healthy" }));

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
