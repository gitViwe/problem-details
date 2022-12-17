using API.Exception;
using Microsoft.AspNetCore.Diagnostics;
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
                int statusCode = StatusCodes.Status500InternalServerError;
                string contentType = "application/problem+json";
                ProblemDetailsFactory problemDetailsFactory = new();
                string response = JsonSerializer.Serialize(problemDetailsFactory.CreateProblemDetailsException(context, statusCode));

                // attempt to get exception details
                var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                if (exceptionFeature is not null)
                {
                    // log error message
                    logger.LogError(exceptionFeature.Error, exceptionFeature.Error.Message);

                    if (exceptionFeature.Error is NotImplementedException)
                    {
                        statusCode = StatusCodes.Status501NotImplemented;
                        response = JsonSerializer.Serialize(problemDetailsFactory.CreateProblemDetailsException(context, statusCode, exceptionFeature.Error.Message));
                    }
                    else if (exceptionFeature.Error is HubValidationException validationException)
                    {
                        statusCode = StatusCodes.Status400BadRequest;
                        response = JsonSerializer.Serialize(problemDetailsFactory.CreateValidationProblemDetails(context, statusCode, validationException.ToDictionary()));
                    }
                }

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = contentType;
                await context.Response.WriteAsync(response);
                await context.Response.CompleteAsync();
            });
        });
    }
}
