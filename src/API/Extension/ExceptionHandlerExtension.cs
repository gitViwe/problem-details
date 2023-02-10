using API.Exception;
using Microsoft.AspNetCore.Diagnostics;
using Shared;
using System.Text.Json;

namespace API.Extension;

public static class ExceptionHandlerExtension
{
    internal static void UseHubExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {
        app.UseExceptionHandler(options =>
        {
            options.Run(async context =>
            {
                var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                string response = JsonSerializer.Serialize(ProblemDetailFactory.CreateProblemDetails(context, StatusCodes.Status500InternalServerError));

                if (exceptionFeature is not null)
                {
                    // log error message
                    logger.LogError(exceptionFeature.Error, exceptionFeature.Error.Message);

                    if (exceptionFeature.Error is NotImplementedException notImplemented)
                    {
                        response = JsonSerializer.Serialize(ProblemDetailFactory.CreateProblemDetails(context, StatusCodes.Status501NotImplemented, exceptionFeature.Error.Message));
                        logger.LogWarning(notImplemented, "The request handler is not fully implemented. Problem detail: {response}", response);
                    }
                    else if (exceptionFeature.Error is HubValidationException validation)
                    {
                        response = JsonSerializer.Serialize(ProblemDetailFactory.CreateValidationProblemDetails(context, StatusCodes.Status400BadRequest, validation.ToDictionary()));
                        logger.LogTrace(validation, "A validation exception occurred. Problem detail: {response}", response);
                    }
                    else
                    {
                        logger.LogError(exceptionFeature.Error, "A unhandled exception occurred. Problem detail: {response}", response);
                    }
                }
                else
                {
                    logger.LogError( "A unhandled exception occurred. Problem detail: {response}", response);
                }

                await context.Response.WriteAsync(response);
                await context.Response.CompleteAsync();
            });
        });
    }
}
