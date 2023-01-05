using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shared;

public interface IProblemDetailFactory
{
    DefaultProblemDetails CreateProblemDetails(HttpContext context, int statusCode, string? detail = null);
    DefaultProblemDetails CreateProblemDetails(HttpContext context, int statusCode, IDictionary<string, object?> extensions, string? detail = null);
    ValidationProblemDetails CreateValidationProblemDetails(HttpContext context, int statusCode, IDictionary<string, string[]> errors, string? detail = null);
}
