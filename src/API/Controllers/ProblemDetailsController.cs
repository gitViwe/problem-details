using API.Exception;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using Shared;
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
            throw new NotImplementedException("This is an exception description that is shared with the calling... expected and handled accordingly.");
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

    [HttpGet("detail")]
    public IActionResult Detail([FromQuery] string detail, [FromQuery] int statusCode)
    {
        return Problem(detail: detail, statusCode: statusCode);
    }

    [HttpPost("with-extension")]
    public IActionResult Result([FromBody] OutOfCreditProblemDetailsInput input)
    {
        var outOfCredit = new OutOfCreditProblemDetails(input.UserBalance, input.UserAccounts, input.ItemCost);

        var problem = ProblemDetailFactory.CreateProblemDetails(
                        context: HttpContext,
                        statusCode: outOfCredit.ToStatus(),
                        extensions: outOfCredit.ToExtension(),
                        detail: outOfCredit.ToDetail());

        return StatusCode(problem.Status!.Value, problem);
    }
}
