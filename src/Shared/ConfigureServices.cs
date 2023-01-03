using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Shared;

public static class ConfigureServices
{
    public static IServiceCollection AddCustomProblemDetailFactory(this IServiceCollection services)
    {
        services.TryAddSingleton<IProblemDetailFactory, ProblemDetailFactory>();
        return services;
    }
}
