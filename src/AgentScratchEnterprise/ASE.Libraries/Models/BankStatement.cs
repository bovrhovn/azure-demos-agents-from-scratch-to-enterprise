namespace ASE.Libraries.Models;

public class BankStatement
{
    public required string AccountNumber { get; set; }
    public decimal Balance { get; set; } = 0;
    public List<BankTransaction> Transactions { get; set; } = [];
}