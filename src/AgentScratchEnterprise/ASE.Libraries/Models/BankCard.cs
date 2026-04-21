namespace ASE.Libraries.Models;

public class BankCard
{
    public required string CardType { get; set; }
    public required string CardNumber { get; set; }
    public required string CardHolderName { get; set; }
    public DateTime ExpirationDate { get; set; }
    public required string CVV { get; set; }
}