using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Shared;

public class DefaultProblemDetails : ProblemDetails
{
    public DefaultProblemDetails(string traceIdentifier, ProblemDetails problemDetails)
    {
        SetProblemDefaults(problemDetails);
        TraceId = traceIdentifier;
    }

    public DefaultProblemDetails(string traceIdentifier, ProblemDetails problemDetails, IDictionary<string, object?> extensions)
        : this(traceIdentifier, problemDetails)
    {
        AddExtensions(extensions);
    }

    [JsonPropertyName("traceId")]
    public string TraceId { get; }

    private void SetProblemDefaults(ProblemDetails problem)
    {
        Status = problem.Status;
        Title = problem.Title;
        Type = problem.Type;

        if (!string.IsNullOrWhiteSpace(problem.Detail))
        {
            Detail = problem.Detail;
        }

        if (!string.IsNullOrWhiteSpace(problem.Instance))
        {
            Instance = problem.Instance;
        }
    }

    private void AddExtensions(IDictionary<string, object?> extensions)
    {
        if (extensions is not null && extensions.Any())
        {
            foreach (var item in extensions)
            {
                Extensions.Add(item);
            }
        }
    }
}