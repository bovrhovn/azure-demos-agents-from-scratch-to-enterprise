using Bogus;
using ASE.Libraries.Models;

namespace ASE.Libraries;

public static class BankDataGenerator
{
    private static readonly Faker<BankCard> CardFaker = new Faker<BankCard>()
        .RuleFor(c => c.CardType, f => f.PickRandom("Visa", "Mastercard", "American Express", "Discover"))
        .RuleFor(c => c.CardNumber, f => f.Finance.CreditCardNumber())
        .RuleFor(c => c.CardHolderName, f => f.Name.FullName())
        .RuleFor(c => c.ExpirationDate, f => f.Date.Future(5))
        .RuleFor(c => c.CVV, f => f.Finance.CreditCardCvv());

    public static BankCard GenerateCard() => CardFaker.Generate();

    public static List<BankCard> GenerateCards(int count = 2) => CardFaker.Generate(count);

    public static BankTransaction GenerateTransaction(BankCard card)
    {
        var transactionFaker = new Faker<BankTransaction>()
            .RuleFor(t => t.TransactionId, f => Guid.NewGuid().ToString())
            .RuleFor(t => t.Date, f => f.Date.Past(2))
            .RuleFor(t => t.Amount, f => f.Finance.Amount(0, 10000))
            .RuleFor(t => t.Description, f => f.Commerce.ProductName())
            .RuleFor(t => t.BankCard, _ => card);
        
        return transactionFaker.Generate();
    }

    public static List<BankTransaction> GenerateTransactions(BankCard card, int count = 10)
    {
        var transactions = new List<BankTransaction>();
        for (int i = 0; i < count; i++)
        {
            transactions.Add(GenerateTransaction(card));
        }
        return transactions;
    }

    public static BankStatement GenerateStatement(string accountNumber, List<BankTransaction> transactions)
    {
        var balance = transactions.Sum(t => t.Amount);
        
        return new BankStatement
        {
            AccountNumber = accountNumber,
            Balance = balance,
            Transactions = transactions
        };
    }

    public static BankData GenerateBankData()
    {
        var faker = new Faker();
        var accountNumber = faker.Finance.Account(10);
        var accountHolderName = faker.Name.FullName();
        
        // Generate 1-3 cards
        var cards = CardFaker.Generate(faker.Random.Int(1, 3));
        
        // Generate transactions for each card (5-20 transactions per card)
        var allTransactions = new List<BankTransaction>();
        foreach (var card in cards)
        {
            var transactionCount = faker.Random.Int(5, 20);
            var transactions = GenerateTransactions(card, transactionCount);
            allTransactions.AddRange(transactions);
        }
        
        // Calculate total balance from all transactions
        var balance = allTransactions.Sum(t => t.Amount);
        
        return new BankData
        {
            AccountNumber = accountNumber,
            AccountHolderName = accountHolderName,
            Balance = balance,
            Cards = cards
        };
    }

    public static (BankData BankData, BankStatement Statement) GenerateBankDataWithStatement()
    {
        var faker = new Faker();
        var accountNumber = faker.Finance.Account(10);
        var accountHolderName = faker.Name.FullName();
        
        // Generate 1-3 cards
        var cards = CardFaker.Generate(faker.Random.Int(1, 3));
        
        // Generate transactions for each card (5-20 transactions per card)
        var allTransactions = new List<BankTransaction>();
        foreach (var card in cards)
        {
            var transactionCount = faker.Random.Int(5, 20);
            var transactions = GenerateTransactions(card, transactionCount);
            allTransactions.AddRange(transactions);
        }
        
        // Calculate total balance from all transactions
        var balance = allTransactions.Sum(t => t.Amount);
        
        var bankData = new BankData
        {
            AccountNumber = accountNumber,
            AccountHolderName = accountHolderName,
            Balance = balance,
            Cards = cards
        };
        
        var statement = GenerateStatement(accountNumber, allTransactions);
        
        return (bankData, statement);
    }

    public static List<BankData> GenerateBankDataList(int count = 10)
    {
        var bankDataList = new List<BankData>();
        for (int i = 0; i < count; i++)
        {
            bankDataList.Add(GenerateBankData());
        }
        return bankDataList;
    }

    public static List<(BankData BankData, BankStatement Statement)> GenerateBankDataWithStatementList(int count = 10)
    {
        var bankDataList = new List<(BankData, BankStatement)>();
        for (int i = 0; i < count; i++)
        {
            bankDataList.Add(GenerateBankDataWithStatement());
        }
        return bankDataList;
    }
}
