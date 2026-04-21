using ASE.Libraries;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Spectre.Console;

#region Environment variables

var endpoint = Environment.GetEnvironmentVariable("ENDPOINT");
ArgumentException.ThrowIfNullOrEmpty(endpoint, "Please set the ENDPOINT environment variable.");
var deploymentName = Environment.GetEnvironmentVariable("DEPLOYMENTNAME");
ArgumentException.ThrowIfNullOrEmpty(deploymentName, "Please set the DEPLOYMENTNAME environment variable.");

#endregion

TextSearchProviderOptions textSearchOptions = new()
{
    SearchTime = TextSearchProviderOptions.TextSearchBehavior.BeforeAIInvoke,
};

IChatClient client =
    new ChatClientBuilder(
            new AzureOpenAIClient(new Uri(endpoint), new DefaultAzureCredential())
                .GetChatClient(deploymentName)
                .AsIChatClient())
        .Build();

AIAgent agent = client
    .AsAIAgent(new ChatClientAgentOptions
    {
        ChatOptions = new()
        {
            Instructions = "You are a helpful support specialist. Answer questions using the provided context and cite the source document when available."
        },
        AIContextProviders = [new TextSearchProvider(SearchAdapter, textSearchOptions)] 
    });
var question = AnsiConsole.Ask<string>("Ask your question",
    "What is the return policy?");
AnsiConsole.MarkupLine("[green]Question:[/]" + question);
AgentSession session = await agent.CreateSessionAsync();
var agentResponse = await agent.RunAsync(question, session);
AnsiConsole.MarkupLine("[green]Answer: [/]" + agentResponse.Text);

static Task<IEnumerable<TextSearchProvider.TextSearchResult>> SearchAdapter(string query, CancellationToken cancellationToken)
{
    var results = DocumentSearchAdapter.Search(query)
        .Select(r => new TextSearchProvider.TextSearchResult
        {
            SourceName = r.SourceName,
            SourceLink = r.SourceLink,
            Text = r.Text
        });
    return Task.FromResult(results);
}