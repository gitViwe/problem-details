using API.ProblemDetail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API;

public class ProblemDetailsFactory : Microsoft.AspNetCore.Mvc.Infrastructure.ProblemDetailsFactory
{
    private const string CONTENT_TYPE = "application/problem+json";

    public ProblemDetails CreateProblemDetails(HttpContext context, int statusCode)
    {
        return CreateProblemDetails(context, statusCode, title: null, type: null, detail: null, instance: context.Request.Path);
    }

    public ValidationProblemDetails CreateValidationProblemDetails(HttpContext context, int statusCode, IDictionary<string, string[]> errors, string? detail = null)
    {
        return CreateValidationProblemDetails(context, errors, statusCode, title: null, type: null, detail, instance: context.Request.Path);
    }

    public ProblemDetailsException CreateProblemDetailsException(HttpContext context, int statusCode, string? detail = null)
    {
        var problemDetails = CreateProblemDetails(context, statusCode, title: null, type: null, detail, instance: context.Request.Path);
        return new ProblemDetailsException(context, problemDetails);
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

    /// <summary>
    /// Creates a <see cref="ValidationProblemDetails" /> instance that configures defaults based on values specified in <see cref="ApiBehaviorOptions" />.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext" />.</param>
    /// <param name="errors">The <see cref="IDictionary{string, string[]}" />.</param>
    /// <param name="statusCode">The value for <see cref="ProblemDetails.Status"/>.</param>
    /// <param name="title">The value for <see cref="ProblemDetails.Title" />.</param>
    /// <param name="type">The value for <see cref="ProblemDetails.Type" />.</param>
    /// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
    /// <param name="instance">The value for <see cref="ProblemDetails.Instance" />.</param>
    /// <returns>The <see cref="ValidationProblemDetails"/> instance.</returns>
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
