using ASE.Libraries.Data;
using ASE.Libraries.Models;

namespace ASE.Libraries.Search;

/// <summary>
/// Provides a mock document-search back-end for the RAG text-search sample.
/// In production this would call an actual search index (e.g. Azure AI Search).
/// </summary>
public class DocumentSearchAdapter : ISearchService
{
    /// <summary>
    /// Returns matching document snippets for the supplied query.
    /// Currently, supports return/refund policy lookups and also bank record check.
    /// </summary>
    /// <returns>
    ///     A list of <see cref="SearchResult"/> objects matching the query.
    /// </returns>
    public List<SearchResult> Search(string query, int records = 10)
    {
        var list = new List<SearchResult>();
        //search policies we have as company
        if (query.Contains("return", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("refund", StringComparison.OrdinalIgnoreCase))
        {
            list.Add(new SearchResult
            {
                SourceName = "Contoso Outdoors Return Policy",
                SourceLink = "https://contoso.com/policies/returns",
                Text = "Customers may return any item within 30 days of delivery. " +
                       "Items should be unused and include original packaging. " +
                       "Refunds are issued to the original payment method within 5 business days of inspection."
            });
        }
        
        //if we are searching for an amount, get all transactions from bank, which are bigger than 1000
        if (query.Contains("amount", StringComparison.OrdinalIgnoreCase))
        {
            var dataWithStatements = BankDataGenerator.GenerateBankDataWithStatementList(records);
            var transactionsAbove1k = dataWithStatements.MaxBy(currentStatement => 
                currentStatement.Statement.Transactions.MaxBy(currentTransaction => currentTransaction.Amount)?.Amount > 1000);
            foreach (var transaction in transactionsAbove1k.Statement.Transactions)
            {
                list.Add(new SearchResult
                {
                    SourceName = transaction.BankCard.CardHolderName,
                    SourceLink =
                        $"https://contoso.com/accounts/{transaction.BankCard.CardNumber}/statements/{transaction.BankCard.CardNumber}",
                    Text =
                        $"Account {transaction.BankCard.CardNumber} has a transaction of amount {transaction.Amount} on {transaction.Date:d} with description '{transaction.Description}'"
                });
            }
        } //after data will be returned, LLM will do the work based on instructions

        return list;
    }
}