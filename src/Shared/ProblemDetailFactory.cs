using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;

namespace Shared;

public class ProblemDetailFactory : Microsoft.AspNetCore.Mvc.Infrastructure.ProblemDetailsFactory, IProblemDetailFactory
{
    private const string CONTENT_TYPE = "application/problem+json";

    public DefaultProblemDetails CreateProblemDetails(HttpContext context, int statusCode, string? detail = null)
    {
        var problemDetails = CreateProblemDetails(context, statusCode, title: null, type: null, detail, instance: context.Request.Path);
        return new DefaultProblemDetails(Activity.Current?.Id ?? context.TraceIdentifier, problemDetails);
    }

    public DefaultProblemDetails CreateProblemDetails(HttpContext context, int statusCode, IDictionary<string, object?> extensions, string? detail = null)
    {
        var problemDetails = CreateProblemDetails(context, statusCode, title: null, type: null, detail, instance: context.Request.Path);
        return new DefaultProblemDetails(Activity.Current?.Id ?? context.TraceIdentifier, problemDetails, extensions);
    }

    public ValidationProblemDetails CreateValidationProblemDetails(HttpContext context, int statusCode, IDictionary<string, string[]> errors, string? detail = null)
    {
        return CreateValidationProblemDetails(context, errors, statusCode, title: null, type: null, detail, instance: context.Request.Path);
    }

    public override ProblemDetails CreateProblemDetails(
        HttpContext context,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        int status = statusCode ?? context.Response.StatusCode;
        context.Response.ContentType = CONTENT_TYPE;

        var result = StatusCodeProblemDetails.Create(status);
        SetProblemDefaults(result, status, title, type, detail, instance: instance ?? context.Request.Path);
        return result;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext context,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        int status = statusCode ?? context.Response.StatusCode;
        context.Response.ContentType = CONTENT_TYPE;

        var result = new ValidationProblemDetails(modelStateDictionary);
        SetProblemDefaults(result, status, title, type, detail, instance);
        return result;
    }

    public ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext context,
        IDictionary<string, string[]> errors,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        int status = statusCode ?? context.Response.StatusCode;
        context.Response.ContentType = CONTENT_TYPE;

        var result = new ValidationProblemDetails(errors);
        SetProblemDefaults(result, status, title, type, detail, instance);
        return result;
    }

    private static void SetProblemDefaults(
            ProblemDetails result,
            int statusCode,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
    {
        result.Status = statusCode;
        result.Type = type ?? result.Type ?? StatusCodeProblemDetails.GetDefaultType(statusCode);

        if (!string.IsNullOrWhiteSpace(title))
        {
            result.Title = title;
        }

        if (!string.IsNullOrWhiteSpace(detail))
        {
            result.Detail = detail;
        }

        if (!string.IsNullOrWhiteSpace(instance))
        {
            result.Instance = instance;
        }
    }
}
