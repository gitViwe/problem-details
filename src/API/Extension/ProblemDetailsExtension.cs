using Microsoft.Extensions.DependencyInjection.Extensions;

namespace API.Extension;

public static class ProblemDetailsExtension
{
    public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
    {
        services.TryAddSingleton<ProblemDetailsFactory>();

        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = context.HttpContext.Request.Path;
                context.ProblemDetails.Type = $"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/{context.ProblemDetails.Status}";
            };
        });

        return services;
    }
}
