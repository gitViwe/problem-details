using FluentValidation.Results;

namespace API.Exception;

/// <summary>
/// A custom validation exception
/// </summary>
public class HubValidationException : System.Exception
{
    private readonly IEnumerable<ValidationFailure> _failures;

    public HubValidationException()
        : base("One or more validation failures have occurred.")
    {
        _failures = Array.Empty<ValidationFailure>();
    }

    public HubValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        _failures = failures;
    }

    /// <summary>
    /// Converts the ValidationResult's errors collection into a simple dictionary representation.
    /// </summary>
    /// <returns>A dictionary keyed by property name
    /// where each value is an array of error messages associated with that property.</returns>
    public IDictionary<string, string[]> ToDictionary()
    {
        return _failures
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );
    }
}
