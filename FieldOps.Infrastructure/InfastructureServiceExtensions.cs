using FieldOps.Application.Interfaces.IRepositories;
using FieldOps.Domain.BouncyCastle;
using FieldOps.Infrastructure.Data;
using FieldOps.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FieldOps.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("BouncyCastleDatabase")
                               ?? throw new InvalidOperationException(
                                   "Connection string 'BouncyCastleDatabase' not found.");
        // Use the DbContext from FieldOps.Infrastructure (no local definition here)
        services.AddDbContext<FieldOpsDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        
        return services;
    }
}