namespace ASE.Libraries.Models;

public class BankData
{
    public required string AccountNumber { get; set; }
    public required string AccountHolderName { get; set; }
    public decimal Balance { get; set; } = 0;
    public List<BankCard> Cards { get; set; } = [];
}