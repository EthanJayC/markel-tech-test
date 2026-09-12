using Markel.Domain.Abstractions;
using Markel.Infrastructure.Persistence;
using Markel.Infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;

namespace Markel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<InMemoryDataStore>();
        services.AddSingleton<ICompanyRepository, InMemoryCompanyRepository>();
        services.AddSingleton<IClaimRepository, InMemoryClaimRepository>();
        return services;
    }
}
