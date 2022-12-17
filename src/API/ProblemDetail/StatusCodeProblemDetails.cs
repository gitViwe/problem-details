using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Mvc;

namespace API.ProblemDetail;

/// <summary>
/// A basic problem details representation for an HTTP status code.
/// It includes default values for <see cref="ProblemDetails.Type"/> and <see cref="ProblemDetails.Title"/>.
/// </summary>
public class StatusCodeProblemDetails : ProblemDetails
{
    /// <summary>
    /// Creates a <see cref="ProblemDetails"/> class based on the <paramref name="statusCode"/>
    /// </summary>
    /// <param name="statusCode">The HTTP status code</param>
    /// <returns>A <see cref="ProblemDetails"/> class with default value for
    /// <see cref="ProblemDetails.Status"/>, <see cref="ProblemDetails.Title"/> and <see cref="ProblemDetails.Type"/></returns>
    public static ProblemDetails Create(int statusCode)
    {
        var details = new ProblemDetails();

        SetDetails(details, statusCode);

        return details;
    }

    private static void SetDetails(ProblemDetails details, int statusCode)
    {
        details.Status = statusCode;
        details.Type = GetDefaultType(statusCode);
        details.Title = ReasonPhrases.GetReasonPhrase(statusCode);
    }

    internal static string GetDefaultType(int statusCode)
    {
        return $"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/{statusCode}";
    }
}
