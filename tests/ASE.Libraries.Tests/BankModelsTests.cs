using ASE.Libraries.Models;

namespace ASE.Libraries.Tests;

public class BankCardTests
{
    [Fact]
    public void BankCard_CanBeCreatedWithRequiredProperties()
    {
        // Arrange & Act
        var card = new BankCard
        {
            CardType = "Visa",
            CardNumber = "4111111111111111",
            CardHolderName = "John Doe",
            CVV = "123",
            ExpirationDate = DateTime.Now.AddYears(3)
        };

        // Assert
        Assert.Equal("Visa", card.CardType);
        Assert.Equal("4111111111111111", card.CardNumber);
        Assert.Equal("John Doe", card.CardHolderName);
        Assert.Equal("123", card.CVV);
    }

    [Fact]
    public void BankCard_ExpirationDate_CanBeSet()
    {
        // Arrange
        var expirationDate = new DateTime(2028, 12, 31);

        // Act
        var card = new BankCard
        {
            CardType = "Mastercard",
            CardNumber = "5500000000000004",
            CardHolderName = "Jane Smith",
            CVV = "456",
            ExpirationDate = expirationDate
        };

        // Assert
        Assert.Equal(expirationDate, card.ExpirationDate);
    }
}

public class BankTransactionTests
{
    [Fact]
    public void BankTransaction_DefaultValues_AreSet()
    {
        // Arrange
        var card = new BankCard
        {
            CardType = "Visa",
            CardNumber = "4111111111111111",
            CardHolderName = "Test User",
            CVV = "123"
        };

        // Act
        var transaction = new BankTransaction
        {
            BankCard = card
        };

        // Assert
        Assert.NotEmpty(transaction.TransactionId);
        Assert.Equal(0, transaction.Amount);
        Assert.Equal(string.Empty, transaction.Description);
        Assert.True(transaction.Date <= DateTime.Now);
    }

    [Fact]
    public void BankTransaction_CanBeCreatedWithAllProperties()
    {
        // Arrange
        var card = new BankCard
        {
            CardType = "Visa",
            CardNumber = "4111111111111111",
            CardHolderName = "Test User",
            CVV = "123"
        };
        var transactionId = Guid.NewGuid().ToString();
        var date = new DateTime(2024, 1, 15);
        var amount = 299.99m;
        var description = "Purchase at Store";

        // Act
        var transaction = new BankTransaction
        {
            TransactionId = transactionId,
            Date = date,
            Amount = amount,
            Description = description,
            BankCard = card
        };

        // Assert
        Assert.Equal(transactionId, transaction.TransactionId);
        Assert.Equal(date, transaction.Date);
        Assert.Equal(amount, transaction.Amount);
        Assert.Equal(description, transaction.Description);
        Assert.Equal(card, transaction.BankCard);
    }

    [Fact]
    public void BankTransaction_TransactionId_IsUniqueByDefault()
    {
        // Arrange
        var card = new BankCard
        {
            CardType = "Visa",
            CardNumber = "4111111111111111",
            CardHolderName = "Test User",
            CVV = "123"
        };

        // Act
        var transaction1 = new BankTransaction { BankCard = card };
        var transaction2 = new BankTransaction { BankCard = card };

        // Assert
        Assert.NotEqual(transaction1.TransactionId, transaction2.TransactionId);
    }
}

public class BankStatementTests
{
    [Fact]
    public void BankStatement_CanBeCreatedWithRequiredProperties()
    {
        // Arrange & Act
        var statement = new BankStatement
        {
            AccountNumber = "1234567890"
        };

        // Assert
        Assert.Equal("1234567890", statement.AccountNumber);
        Assert.Equal(0, statement.Balance);
        Assert.Empty(statement.Transactions);
    }

    [Fact]
    public void BankStatement_CanHoldTransactions()
    {
        // Arrange
        var card = new BankCard
        {
            CardType = "Visa",
            CardNumber = "4111111111111111",
            CardHolderName = "Test User",
            CVV = "123"
        };
        
        var transactions = new List<BankTransaction>
        {
            new() { Amount = 100m, BankCard = card },
            new() { Amount = 200m, BankCard = card }
        };

        // Act
        var statement = new BankStatement
        {
            AccountNumber = "1234567890",
            Balance = 300m,
            Transactions = transactions
        };

        // Assert
        Assert.Equal(2, statement.Transactions.Count);
        Assert.Equal(300m, statement.Balance);
    }
}

public class BankDataTests
{
    [Fact]
    public void BankData_CanBeCreatedWithRequiredProperties()
    {
        // Arrange & Act
        var bankData = new BankData
        {
            AccountNumber = "1234567890",
            AccountHolderName = "John Doe"
        };

        // Assert
        Assert.Equal("1234567890", bankData.AccountNumber);
        Assert.Equal("John Doe", bankData.AccountHolderName);
        Assert.Equal(0, bankData.Balance);
        Assert.Empty(bankData.Cards);
    }

    [Fact]
    public void BankData_CanHoldMultipleCards()
    {
        // Arrange
        var cards = new List<BankCard>
        {
            new()
            {
                CardType = "Visa",
                CardNumber = "4111111111111111",
                CardHolderName = "John Doe",
                CVV = "123"
            },
            new()
            {
                CardType = "Mastercard",
                CardNumber = "5500000000000004",
                CardHolderName = "John Doe",
                CVV = "456"
            }
        };

        // Act
        var bankData = new BankData
        {
            AccountNumber = "1234567890",
            AccountHolderName = "John Doe",
            Balance = 5000m,
            Cards = cards
        };

        // Assert
        Assert.Equal(2, bankData.Cards.Count);
        Assert.Equal(5000m, bankData.Balance);
    }

    [Fact]
    public void BankData_Balance_CanBeSet()
    {
        // Arrange
        var balance = 12345.67m;

        // Act
        var bankData = new BankData
        {
            AccountNumber = "1234567890",
            AccountHolderName = "Jane Smith",
            Balance = balance
        };

        // Assert
        Assert.Equal(balance, bankData.Balance);
    }
}
