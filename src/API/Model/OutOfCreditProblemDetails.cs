namespace API.Model;

public record OutOfCreditProblemDetailsInput(int UserBalance, string[] UserAccounts, int ItemCost);
public class OutOfCreditProblemDetails
{
    private readonly int _itemCost;

    public OutOfCreditProblemDetails(int userBalance, string[] userAccounts, int itemCost)
    {
        Balance = userBalance;
        Accounts = userAccounts;
        _itemCost = itemCost;
    }

    public int Balance { get; }
    public IEnumerable<string> Accounts { get; }
    public Dictionary<string, object?> ToExtension() => new Dictionary<string, object?>() { { nameof(OutOfCreditProblemDetails), this } };
    public string ToDetail() => $"Your current balance is {Balance}, but that costs {_itemCost}.";
    public int ToStatus() => 412; // Status412PreconditionFailed
}
