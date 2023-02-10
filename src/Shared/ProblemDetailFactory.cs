using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;

namespace Shared;

/// <summary>
/// A custom factory to produce <see cref="ProblemDetails"/> and <see cref="ValidationProblemDetails"/>.
/// </summary>
public static class ProblemDetailFactory
{
    private const string CONTENT_TYPE = "application/problem+json";

    /// <summary>
    /// Creates a <see cref="DefaultProblemDetails"/> instance that configures defaults for <br></br>
    /// <see cref="ProblemDetails.Status"/><br></br>
    /// <see cref="ProblemDetails.Type"/><br></br>
    /// <see cref="ProblemDetails.Title"/><br></br>
    /// <see cref="HttpResponse.ContentType"/><br></br>
    /// <see cref="HttpResponse.StatusCode"/>
    /// </summary>
    /// <param name="context">The <see cref="HttpContext" />.</param>
    /// <param name="statusCode">The value for <see cref="ProblemDetails.Status"/>.</param>
    /// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
    /// <returns>A custom <see cref="DefaultProblemDetails"/> class</returns>
    public static DefaultProblemDetails CreateProblemDetails(HttpContext context, int statusCode, string? detail = null)
    {
        var problemDetails = CreateProblemDetails(context, statusCode, title: null, type: null, detail, instance: context.Request.Path);
        return new DefaultProblemDetails(Activity.Current?.Id ?? context.TraceIdentifier, problemDetails);
    }

    /// <summary>
    /// Creates a <see cref="DefaultProblemDetails"/> instance that configures defaults for <br></br>
    /// <see cref="ProblemDetails.Status"/><br></br>
    /// <see cref="ProblemDetails.Type"/><br></br>
    /// <see cref="ProblemDetails.Title"/><br></br>
    /// <see cref="HttpResponse.ContentType"/><br></br>
    /// <see cref="HttpResponse.StatusCode"/>
    /// </summary>
    /// <param name="context">The <see cref="HttpContext" />.</param>
    /// <param name="statusCode">The value for <see cref="ProblemDetails.Status"/>.</param>
    /// <param name="extensions">The object extension associated with this instance of <see cref="DefaultProblemDetails"/></param>
    /// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
    /// <returns>A custom <see cref="DefaultProblemDetails"/> class</returns>
    public static DefaultProblemDetails CreateProblemDetails(HttpContext context, int statusCode, IDictionary<string, object?> extensions, string? detail = null)
    {
        var problemDetails = CreateProblemDetails(context, statusCode, title: null, type: null, detail, instance: context.Request.Path);
        return new DefaultProblemDetails(Activity.Current?.Id ?? context.TraceIdentifier, problemDetails, extensions);
    }

    /// <summary>
    /// Creates a <see cref="ValidationProblemDetails"/> instance that configures defaults for <br></br>
    /// <see cref="ProblemDetails.Status"/><br></br>
    /// <see cref="ProblemDetails.Type"/><br></br>
    /// <see cref="ProblemDetails.Title"/><br></br>
    /// <see cref="HttpResponse.ContentType"/><br></br>
    /// <see cref="HttpResponse.StatusCode"/>
    /// </summary>
    /// <param name="context">The <see cref="HttpContext" />.</param>
    /// <param name="statusCode">The value for <see cref="ProblemDetails.Status"/>.</param>
    /// <param name="errors">The errors associated with this instance of <see cref="ValidationProblemDetails"/></param>
    /// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
    /// <returns>A custom <see cref="ValidationProblemDetails"/> class</returns>
    public static ValidationProblemDetails CreateValidationProblemDetails(HttpContext context, int statusCode, IDictionary<string, string[]> errors, string? detail = null)
    {
        return CreateValidationProblemDetails(context, errors, statusCode, title: null, type: null, detail, instance: context.Request.Path);
    }

    /// <summary>
    /// Creates a <see cref="ValidationProblemDetails"/> instance that configures defaults for <br></br>
    /// <see cref="ProblemDetails.Status"/><br></br>
    /// <see cref="ProblemDetails.Type"/><br></br>
    /// <see cref="ProblemDetails.Title"/><br></br>
    /// <see cref="HttpResponse.ContentType"/><br></br>
    /// <see cref="HttpResponse.StatusCode"/>
    /// </summary>
    /// <param name="context">The <see cref="HttpContext" />.</param>
    /// <param name="statusCode">The value for <see cref="ProblemDetails.Status"/>.</param>
    /// <param name="modelStateDictionary">The errors associated with this instance of <see cref="ValidationProblemDetails"/></param>
    /// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
    /// <returns>A custom <see cref="ValidationProblemDetails"/> class</returns>
    public static ValidationProblemDetails CreateValidationProblemDetails(HttpContext context, int statusCode, ModelStateDictionary modelStateDictionary, string? detail = null)
    {
        return CreateValidationProblemDetails(context, modelStateDictionary, statusCode, title: null, type: null, detail, instance: context.Request.Path);
    }

    private static ProblemDetails CreateProblemDetails(
        HttpContext context,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        int status = statusCode ?? context.Response.StatusCode;
        context.Response.ContentType = CONTENT_TYPE;
        context.Response.StatusCode = status;

        var result = StatusCodeProblemDetails.Create(status);
        SetProblemDefaults(result, status, title, type, detail, instance: instance ?? context.Request.Path);
        return result;
    }

    private static ValidationProblemDetails CreateValidationProblemDetails(
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
        context.Response.StatusCode = status;

        var result = new ValidationProblemDetails(modelStateDictionary);
        SetProblemDefaults(result, status, title, type, detail, instance);
        return result;
    }

    private static ValidationProblemDetails CreateValidationProblemDetails(
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
        context.Response.StatusCode = status;

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
