using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Serialization;

namespace API.ProblemDetail;

public class ProblemDetailsException : ProblemDetails
{
    public ProblemDetailsException(HttpContext context, ProblemDetails problemDetails)
    {
        SetProblemDefaults(problemDetails);
        TraceId = context.TraceIdentifier;
    }

    [JsonPropertyName("traceId")]
    public string TraceId { get; }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine($"Type    : {Type}");
        stringBuilder.AppendLine($"Title   : {Title}");
        stringBuilder.AppendLine($"Status  : {Status}");
        stringBuilder.AppendLine($"Detail  : {Detail}");
        stringBuilder.AppendLine($"Instance: {Instance}");

        return stringBuilder.ToString();
    }

    private void SetProblemDefaults(ProblemDetails result)
    {
        this.Status = result.Status;
        this.Title = result.Title;
        this.Type = result.Type;
        this.Detail = result.Detail;
        this.Instance = result.Instance;
    }
}
