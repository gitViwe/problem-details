using Microsoft.AspNetCore.Mvc;

namespace API.ProblemDetail;

public class OutOfCreditProblemDetails : ProblemDetails
{
    public OutOfCreditProblemDetails()
    {
        Accounts = new List<string>();
    }

    public decimal Balance { get; set; }

    public ICollection<string> Accounts { get; }
}
