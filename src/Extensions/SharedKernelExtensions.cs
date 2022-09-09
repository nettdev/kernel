using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Mobnet.SharedKernel;

[ExcludeFromCodeCoverage]
public static class SharedKernelExtensions
{
    public static void AddSharedKernel(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
    }
}