using ASE.Libraries;
using ASE.Libraries.Data;
using ASE.Libraries.Models;

namespace ASE.Libraries.Tests;

public class BankDataGeneratorTests
{
    [Fact]
    public void GenerateCard_ReturnsValidBankCard()
    {
        // Act
        var card = BankDataGenerator.GenerateCard();

        // Assert
        Assert.NotNull(card);
        Assert.NotEmpty(card.CardType);
        Assert.NotEmpty(card.CardNumber);
        Assert.NotEmpty(card.CardHolderName);
        Assert.NotEmpty(card.CVV);
        Assert.True(card.ExpirationDate > DateTime.Now);
    }

    [Fact]
    public void GenerateCard_CardTypeIsValid()
    {
        // Act
        var card = BankDataGenerator.GenerateCard();

        // Assert
        var validCardTypes = new[] { "Visa", "Mastercard", "American Express", "Discover" };
        Assert.Contains(card.CardType, validCardTypes);
    }

    [Fact]
    public void GenerateCards_WithCount_ReturnsCorrectNumberOfCards()
    {
        // Arrange
        var count = 5;

        // Act
        var cards = BankDataGenerator.GenerateCards(count);

        // Assert
        Assert.NotNull(cards);
        Assert.Equal(count, cards.Count);
    }

    [Fact]
    public void GenerateCards_DefaultCount_ReturnsTwoCards()
    {
        // Act
        var cards = BankDataGenerator.GenerateCards();

        // Assert
        Assert.NotNull(cards);
        Assert.Equal(2, cards.Count);
    }

    [Fact]
    public void GenerateTransaction_WithCard_ReturnsValidTransaction()
    {
        // Arrange
        var card = BankDataGenerator.GenerateCard();

        // Act
        var transaction = BankDataGenerator.GenerateTransaction(card);

        // Assert
        Assert.NotNull(transaction);
        Assert.NotEmpty(transaction.TransactionId);
        Assert.True(transaction.Amount >= 0);
        Assert.NotEmpty(transaction.Description);
        Assert.Equal(card, transaction.BankCard);
    }

    [Fact]
    public void GenerateTransactions_WithCount_ReturnsCorrectNumber()
    {
        // Arrange
        var card = BankDataGenerator.GenerateCard();
        var count = 15;

        // Act
        var transactions = BankDataGenerator.GenerateTransactions(card, count);

        // Assert
        Assert.NotNull(transactions);
        Assert.Equal(count, transactions.Count);
    }

    [Fact]
    public void GenerateTransactions_AllTransactionsHaveSameCard()
    {
        // Arrange
        var card = BankDataGenerator.GenerateCard();
        var count = 10;

        // Act
        var transactions = BankDataGenerator.GenerateTransactions(card, count);

        // Assert
        Assert.All(transactions, t => Assert.Equal(card, t.BankCard));
    }

    [Fact]
    public void GenerateStatement_CalculatesCorrectBalance()
    {
        // Arrange
        var card = BankDataGenerator.GenerateCard();
        var transactions = BankDataGenerator.GenerateTransactions(card, 10);
        var accountNumber = "1234567890";

        // Act
        var statement = BankDataGenerator.GenerateStatement(accountNumber, transactions);

        // Assert
        Assert.NotNull(statement);
        Assert.Equal(accountNumber, statement.AccountNumber);
        Assert.Equal(transactions.Sum(t => t.Amount), statement.Balance);
        Assert.Equal(transactions, statement.Transactions);
    }

    [Fact]
    public void GenerateBankData_ReturnsValidBankData()
    {
        // Act
        var bankData = BankDataGenerator.GenerateBankData();

        // Assert
        Assert.NotNull(bankData);
        Assert.NotEmpty(bankData.AccountNumber);
        Assert.NotEmpty(bankData.AccountHolderName);
        Assert.True(bankData.Balance >= 0);
        Assert.NotEmpty(bankData.Cards);
        Assert.InRange(bankData.Cards.Count, 1, 3);
    }

    [Fact]
    public void GenerateBankDataWithStatement_ReturnsBankDataAndStatement()
    {
        // Act
        var (bankData, statement) = BankDataGenerator.GenerateBankDataWithStatement();

        // Assert
        Assert.NotNull(bankData);
        Assert.NotNull(statement);
        Assert.Equal(bankData.AccountNumber, statement.AccountNumber);
        Assert.Equal(bankData.Balance, statement.Balance);
    }

    [Fact]
    public void GenerateBankDataList_ReturnsCorrectCount()
    {
        // Arrange
        var count = 7;

        // Act
        var bankDataList = BankDataGenerator.GenerateBankDataList(count);

        // Assert
        Assert.NotNull(bankDataList);
        Assert.Equal(count, bankDataList.Count);
    }

    [Fact]
    public void GenerateBankDataList_DefaultCount_ReturnsTenItems()
    {
        // Act
        var bankDataList = BankDataGenerator.GenerateBankDataList();

        // Assert
        Assert.NotNull(bankDataList);
        Assert.Equal(10, bankDataList.Count);
    }

    [Fact]
    public void GenerateBankDataWithStatementList_ReturnsCorrectCount()
    {
        // Arrange
        var count = 5;

        // Act
        var bankDataList = BankDataGenerator.GenerateBankDataWithStatementList(count);

        // Assert
        Assert.NotNull(bankDataList);
        Assert.Equal(count, bankDataList.Count);
        Assert.All(bankDataList, item =>
        {
            Assert.NotNull(item.BankData);
            Assert.NotNull(item.Statement);
        });
    }

    [Fact]
    public void GenerateBankDataWithStatementList_DefaultCount_ReturnsTenItems()
    {
        // Act
        var bankDataList = BankDataGenerator.GenerateBankDataWithStatementList();

        // Assert
        Assert.NotNull(bankDataList);
        Assert.Equal(10, bankDataList.Count);
    }
}
