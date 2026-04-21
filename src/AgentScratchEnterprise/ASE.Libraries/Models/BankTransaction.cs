namespace ASE.Libraries.Models;

public class BankTransaction
{
    public string TransactionId { get; set; } = Guid.NewGuid().ToString();
    public DateTime Date { get; set; } = DateTime.Now;
    public decimal Amount { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public required BankCard BankCard { get; set; }
}