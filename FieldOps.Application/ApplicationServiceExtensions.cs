

using FieldOps.Application.BouncyCastleApp;
using FieldOps.Application.Interfaces.IServices;
using Microsoft.Extensions.DependencyInjection;

namespace FieldOps.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IBouncyCastleService, BouncyCastleService>();
        
        return services;
    }
}