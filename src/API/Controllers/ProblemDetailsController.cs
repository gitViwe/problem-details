using API.Exception;
using API.Model;
using API.ProblemDetail;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace API.Controllers
{
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

        [HttpGet("detail")]
        public IActionResult Detail([FromServices] ProblemDetailsFactory problemDetailsFactory)
        {
            var problem = problemDetailsFactory
                .CreateProblemDetails(HttpContext, StatusCodes.Status412PreconditionFailed, detail: "This will end up in the 'detail' field.");
            return BadRequest(problem);
        }

        [HttpGet("custom-problem")]
        public IActionResult Result()
        {
            var problem = new OutOfCreditProblemDetails
            {
                Type = "https://example.com/probs/out-of-credit",
                Title = "You do not have enough credit.",
                Detail = "Your current balance is 30, but that costs 50.",
                Instance = "/account/12345/msgs/abc",
                Balance = 30.0m,
                Accounts = { "/account/12345", "/account/67890" }
            };

            return BadRequest(problem);
        }
    }
}
