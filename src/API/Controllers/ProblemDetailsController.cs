using API.Exception;
using API.Model;
using API.ProblemDetail;
using gitViwe.ProblemDetail;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProblemDetailsController : ControllerBase
{
    [HttpGet("status/{statusCode}")]
    public IActionResult Status([FromRoute] int statusCode)
    {
        return StatusCode(statusCode);
    }

    [HttpGet("exception/{isHandled}")]
    public IActionResult Exception([FromRoute] bool isHandled)
    {
        if (isHandled)
        {
            throw new NotImplementedException("This is an exception thrown from an API controller.");
        }

        throw new System.Exception("This is an exception thrown from an API controller.");
    }

    [HttpPost("validation-error")]
    [Consumes(MediaTypeNames.Application.Json)]
    public IActionResult ErrorDetails([FromBody] InputModel input)
    {
        var result = new InputModelValidator().Validate(input);

        if (result.IsValid)
        {
            var createdResource = input;
            var actionName = nameof(ErrorDetails);
            var routeValues = new { id = 245, version = 1 };
            return CreatedAtAction(actionName, routeValues, createdResource);
        }

        throw new HubValidationException(result.Errors);
    }

    [HttpGet("detail/{detail}")]
    public IActionResult Detail([FromRoute] string detail)
    {
        return Problem(detail: detail, statusCode: StatusCodes.Status400BadRequest);
    }

    [HttpGet("custom-problem")]
    public IActionResult Result([FromServices] IProblemDetailFactory problemDetailFactory)
    {
        var extensionValue = new OutOfCreditProblemDetails
        {
            Balance = 30.0m,
            Accounts = { "/account/12345", "/account/67890" }
        };

        var extensionKey = char.ToLowerInvariant(nameof(OutOfCreditProblemDetails)[0]) + nameof(OutOfCreditProblemDetails)[1..];

        var problem = problemDetailFactory.CreateProblemDetails(
                        context: HttpContext,
                        statusCode: StatusCodes.Status412PreconditionFailed,
                        extensions: new Dictionary<string, object?>()
                        { 
                            { extensionKey, extensionValue }
                        },
                        detail: "Your current balance is 30, but that costs 50.");

        return StatusCode(problem.Status!.Value, problem);
    }
}
