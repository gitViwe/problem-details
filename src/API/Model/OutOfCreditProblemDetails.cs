namespace API.Model;

public class OutOfCreditProblemDetails
{
    public decimal Balance { get; set; }

    public ICollection<string> Accounts { get; } = new List<string>();
}
