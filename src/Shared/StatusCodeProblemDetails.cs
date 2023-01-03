using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Shared;

internal class StatusCodeProblemDetails
{
    internal static ProblemDetails Create(int statusCode)
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
