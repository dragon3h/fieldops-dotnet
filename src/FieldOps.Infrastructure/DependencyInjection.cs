using System;
using FieldOps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Infrastructure.Repositories;

namespace FieldOps.Infrastructure
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
      // Resolve connection string deterministically from configuration, falling back to env var.
      var connectionString = configuration.GetConnectionString("BouncyCastleDatabase")
                             ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                             ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

      // Register DbContext with the resolved connection string and transient-fault retries.
      services.AddDbContext<FieldOpsDbContext>(options =>
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
          npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorCodesToAdd: null);
        }));

      // Register specific repositories
      services.AddScoped<IBouncyCastleRepository, BouncyCastleRepository>();
      services.AddScoped<IClientRepository, ClientRepository>();

      // Register generic repository and unit of work
      services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
      services.AddScoped<IUnitOfWork, UnitOfWork>();

      return services;
    }
  }
}