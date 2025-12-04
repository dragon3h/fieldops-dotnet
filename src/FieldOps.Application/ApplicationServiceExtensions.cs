using FieldOps.Application.BouncyCastleApp;
using FieldOps.Application.ClientApp;
using FieldOps.Application.Interfaces.IServices;
using FieldOps.Application.MappingProfiles;
using FieldOps.Domain.BouncyCastle;
using Microsoft.Extensions.DependencyInjection;

namespace FieldOps.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper with license configuration
        services.AddAutoMapper(cfg =>
        {
            // Community license (free for < $5M revenue)
            // Register at https://automapper.io
            cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzk1OTk2ODAwIiwiaWF0IjoiMTc2NDUzNDcwNiIsImFjY291bnRfaWQiOiIwMTlhZDY3NjUzODA3ZmIyYTdhNzcxNTE0ZmY4MjU5YiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2JiN2RyYTl0NzB5ZzZlcXJiOXJ6bTFmIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.trAVlOFUOwm33WqH_ANv9G8gGgSX1JPkBbAIVXB9Eyrto7J3UjKaxIunfOs_kF6jk3woWgKGgRQ37KYYd1lGK5Myrxc2l-A_Phy6YnFQqoqa6Ps-F32CB9UkBky_c0eppNkuTdrt28rOx1mrjwhHVgXeFs_DCSKIqX32AlNsYAzh_f6djM2n4rPt24EFl1sepxtj_D8XiideqwyJczyY6wNIW9cxMeX5M6Rdv_UlO8cP91DX69MrRDUwvcfpt8d2jNh4xjKFWiq-1GDkfrdzNnzpc-WweIixiAh8e8VACPezkUp7usNNHFk8k5i4orm0BxQjMBamSdlIxOeTkLva8g";
        }, new[] { typeof(BouncyCastleProfile).Assembly, typeof(ClientProfile).Assembly });

        services.AddScoped<IBouncyCastleService, BouncyCastleService>();
        services.AddScoped<IClientService, ClientService>();

        return services;
    }
}