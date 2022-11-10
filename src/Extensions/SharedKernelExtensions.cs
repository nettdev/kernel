using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Nett.SharedKernel;

[ExcludeFromCodeCoverage]
public static class SharedKernelExtensions
{
    public static void AddSharedKernel(this IServiceCollection services)
    {
        services.AddScoped<ITokenManager, TokenManager>();
    }
}