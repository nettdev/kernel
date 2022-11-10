using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Nett.Kernel;

[ExcludeFromCodeCoverage]
public static class KernelExtensions
{
    public static void AddKernel(this IServiceCollection services)
    {
        services.AddScoped<ITokenManager, TokenManager>();
    }
}