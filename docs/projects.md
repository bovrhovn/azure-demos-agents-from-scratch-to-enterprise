# 📦 Projects Documentation

Detailed documentation for each project in the Azure AI Agents solution.

---

## 🔍 Project Overview

The solution contains three main projects that demonstrate the evolution from basic to enterprise-ready AI agents:

```
src/AgentScratchEnterprise/
├── ASE.SimpleAgent/           ← Simple AI agent
├── ASE.SimpleAgentSearch/     ← Agent with RAG search
└── ASE.Libraries/             ← Shared components
```

---

## 1. ASE.SimpleAgent 🤖

### Purpose
A foundational AI agent that demonstrates basic question-answering using Azure AI Projects SDK.

### Key Features
- ✅ Direct Azure AI Projects integration
- ✅ Simple conversational interface
- ✅ Azure authentication via DefaultAzureCredential
- ✅ Rich console UI with Spectre.Console

### Project Structure
```
ASE.SimpleAgent/
├── Program.cs                 ← Main application
└── ASE.SimpleAgent.csproj     ← Project configuration
```

### Dependencies
| Package | Version | Purpose |
|---------|---------|---------|
| `Azure.AI.Projects` | 2.0.0 | Azure AI Foundry SDK |
| `Azure.Identity` | 1.21.0 | Azure authentication |
| `Microsoft.Agents.AI.Foundry` | 1.1.0 | Agent abstractions |
| `Spectre.Console` | 0.55.0 | Rich console UI |
| `ModelContextProtocol.Core` | 1.2.0 | MCP support |

### Code Walkthrough

#### 1. Environment Configuration
```csharp
var endpoint = Environment.GetEnvironmentVariable("ENDPOINT");
ArgumentException.ThrowIfNullOrEmpty(endpoint, "Please set the ENDPOINT environment variable.");

var deploymentName = Environment.GetEnvironmentVariable("DEPLOYMENTNAME");
ArgumentException.ThrowIfNullOrEmpty(deploymentName, "Please set the DEPLOYMENTNAME environment variable.");
```

**What it does:** Validates required configuration from environment variables.

---

#### 2. Agent Creation
```csharp
AIAgent agent = new AIProjectClient(new Uri(endpoint), new DefaultAzureCredential())
    .AsAIAgent(
        model: deploymentName,
        instructions: "You are a friendly assistant. Keep your answers brief.",
        name: "SimpleAgentToStartWith");
```

**Components:**
- `AIProjectClient` - Connects to Azure AI Foundry project
- `DefaultAzureCredential` - Handles authentication automatically
- `AsAIAgent()` - Extension method to create agent from client
- `instructions` - System prompt defining agent behavior
- `name` - Identifier for the agent

---

#### 3. User Interaction
```csharp
var question = AnsiConsole.Ask<string>("Ask your question",
    "What is the 2nd largest city in Poland by population size?");
AnsiConsole.MarkupLine("[green]Question:[/]" + question);
```

**UI Features:**
- Interactive prompt with default value
- Color-coded output ([green] for questions)
- User-friendly console experience

---

#### 4. Agent Execution
```csharp
var answer = await agent.RunAsync(question);
AnsiConsole.MarkupLine("[green]Answer:[/]" + answer);
```

**Execution Flow:**
1. User input sent to agent
2. Agent forwards to Azure OpenAI
3. Response received and formatted
4. Answer displayed to user

### Usage Examples

**Basic Q&A:**
```
Ask your question [What is the 2nd largest city in Poland by population size?]: 
Question: What is the 2nd largest city in Poland by population size?
Answer: The second-largest city in Poland by population is Kraków, with approximately 800,000 residents.
```

**Custom Question:**
```
Ask your question: Explain quantum computing in simple terms
Question: Explain quantum computing in simple terms
Answer: Quantum computing uses quantum bits (qubits) that can be 0, 1, or both simultaneously, enabling parallel processing of multiple possibilities at once.
```

### Running the Project

```bash
cd src\AgentScratchEnterprise\ASE.SimpleAgent
dotnet run
```

### Customization

**Change Agent Personality:**
```csharp
instructions: "You are an expert software engineer. Provide detailed technical explanations with code examples."
```

**Change Model:**
```csharp
model: "gpt-4o-mini"  // Faster, more cost-effective
```

📖 **Microsoft Learn:** [Your First Agent](https://learn.microsoft.com/agent-framework/get-started/your-first-agent)

---

## 2. ASE.SimpleAgentSearch 🔍

### Purpose
An enhanced agent demonstrating RAG (Retrieval-Augmented Generation) with document search capabilities.

### Key Features
- ✅ Text search provider integration
- ✅ RAG pattern implementation
- ✅ Document citation in responses
- ✅ Session-based conversations
- ✅ Context-aware AI responses

### Project Structure
```
ASE.SimpleAgentSearch/
├── Program.cs                        ← Main application with search
└── ASE.SimpleAgentSearch.csproj      ← Project configuration
```

### Dependencies
| Package | Version | Purpose |
|---------|---------|---------|
| `Azure.AI.OpenAI` | Latest | OpenAI API client |
| `Azure.Identity` | 1.21.0 | Authentication |
| `Microsoft.Agents.AI` | Latest | Agent framework |
| `Microsoft.Extensions.AI` | Latest | AI abstractions |
| `Spectre.Console` | 0.55.0 | Console UI |
| `ASE.Libraries` | - | Search adapter |

### Code Walkthrough

#### 1. Search Configuration
```csharp
TextSearchProviderOptions textSearchOptions = new()
{
    SearchTime = TextSearchProviderOptions.TextSearchBehavior.BeforeAIInvoke,
};
```

**Search Strategies:**
- `BeforeAIInvoke` - Search documents before generating answer
- `AfterAIInvoke` - Generate answer first, then validate with search
- `None` - Disable search

---

#### 2. Chat Client Setup
```csharp
IChatClient client =
    new ChatClientBuilder(
            new AzureOpenAIClient(new Uri(endpoint), new DefaultAzureCredential())
                .GetChatClient(deploymentName)
                .AsIChatClient())
        .Build();
```

**Architecture:**
- `AzureOpenAIClient` - Low-level OpenAI API access
- `GetChatClient()` - Creates chat-specific client
- `AsIChatClient()` - Converts to standard interface
- `ChatClientBuilder` - Adds extensions and middleware

---

#### 3. Agent with Search Provider
```csharp
AIAgent agent = client
    .AsAIAgent(new ChatClientAgentOptions
    {
        ChatOptions = new()
        {
            Instructions = "You are a helpful support specialist. Answer questions using the provided context and cite the source document when available."
        },
        AIContextProviders = [new TextSearchProvider(SearchAdapter, textSearchOptions)] 
    });
```

**Components:**
- `ChatClientAgentOptions` - Configuration for agent behavior
- `ChatOptions` - AI model parameters and instructions
- `AIContextProviders` - List of context providers (search, tools, etc.)
- `TextSearchProvider` - Implements RAG pattern

---

#### 4. Search Adapter Implementation
```csharp
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
```

**Transformation:**
- Calls `DocumentSearchAdapter.Search()`
- Converts `SearchResult` to `TextSearchResult`
- Returns results asynchronously

---

#### 5. Session Management
```csharp
AgentSession session = await agent.CreateSessionAsync();
var agentResponse = await agent.RunAsync(question, session);
AnsiConsole.MarkupLine("[green]Answer: [/]" + agentResponse.Text);
```

**Session Benefits:**
- Maintains conversation history
- Context across multiple turns
- Efficient token usage

### Usage Examples

**Policy Query with Citation:**
```
Ask your question [What is the return policy?]: 
Question: What is the return policy?
Answer: According to the Contoso Outdoors Return Policy, customers may return any item within 30 days of delivery. Items should be unused and include original packaging. Refunds are issued to the original payment method within 5 business days of inspection.

Source: Contoso Outdoors Return Policy (https://contoso.com/policies/returns)
```

**Query Without Matching Documents:**
```
Ask your question: What is the weather today?
Question: What is the weather today?
Answer: I don't have access to weather information in my knowledge base. Please check a weather service for current conditions.
```

### Running the Project

```bash
cd src\AgentScratchEnterprise\ASE.SimpleAgentSearch
dotnet run
```

### Extending Search Capabilities

Replace the mock `DocumentSearchAdapter` with real search:

```csharp
// Azure AI Search integration
static async Task<IEnumerable<TextSearchProvider.TextSearchResult>> SearchAdapter(string query, CancellationToken ct)
{
    var searchClient = new SearchClient(searchEndpoint, indexName, credential);
    var results = await searchClient.SearchAsync<Document>(query, cancellationToken: ct);
    
    return results.Value.GetResults().Select(r => new TextSearchProvider.TextSearchResult
    {
        SourceName = r.Document.Title,
        SourceLink = r.Document.Url,
        Text = r.Document.Content
    });
}
```

📖 **Microsoft Learn:** [RAG with Text Search](https://learn.microsoft.com/agent-framework/concepts/rag)

---

## 3. ASE.Libraries 📦

### Purpose
Shared library containing reusable components for search and data access.

### Key Features
- ✅ Mock document search backend
- ✅ Extensible adapter pattern
- ✅ Search result models
- ✅ 100% test coverage

### Project Structure
```
ASE.Libraries/
├── DocumentSearchAdapter.cs      ← Search implementation
└── ASE.Libraries.csproj           ← Project configuration
```

### Components

#### `SearchResult` Class

```csharp
public sealed class SearchResult
{
    public string SourceName { get; init; } = string.Empty;
    public string SourceLink { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
}
```

**Properties:**
- `SourceName` - Human-readable document name
- `SourceLink` - URL to source document
- `Text` - Content snippet matching the query

**Immutability:**
- `init` setters ensure thread-safety
- Cannot be modified after creation
- Safe for concurrent access

---

#### `DocumentSearchAdapter` Class

```csharp
public static class DocumentSearchAdapter
{
    public static IEnumerable<SearchResult> Search(string query)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (query.Contains("return", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("refund", StringComparison.OrdinalIgnoreCase))
        {
            yield return new SearchResult
            {
                SourceName = "Contoso Outdoors Return Policy",
                SourceLink = "https://contoso.com/policies/returns",
                Text = "Customers may return any item within 30 days of delivery..."
            };
        }
    }
}
```

**Implementation Details:**
- Static class (no instantiation needed)
- Case-insensitive keyword matching
- Uses `yield return` for lazy evaluation
- Null-safe with `ArgumentNullException`

**Current Limitations:**
- ⚠️ Mock implementation with hardcoded data
- ⚠️ Only supports "return" and "refund" keywords
- ⚠️ No ranking or relevance scoring

**Production Readiness:**
Replace with:
- Azure AI Search
- Azure Cognitive Search
- Azure Cosmos DB with full-text search
- SQL Server full-text search
- Elasticsearch

### Integration Example

```csharp
// Use in your application
var query = "What is your return policy?";
var results = DocumentSearchAdapter.Search(query);

foreach (var result in results)
{
    Console.WriteLine($"Source: {result.SourceName}");
    Console.WriteLine($"Link: {result.SourceLink}");
    Console.WriteLine($"Content: {result.Text}");
}
```

### Testing

The library has **17 comprehensive unit tests** covering:
- ✅ Keyword matching (return, refund)
- ✅ Case sensitivity
- ✅ Edge cases (null, empty strings)
- ✅ Error handling
- ✅ Data model initialization

Run tests:
```bash
cd tests\ASE.Libraries.Tests
dotnet test
```

📖 **More details:** [Testing Documentation](./testing.md)

---

## 🔗 Project Dependencies Graph

```
ASE.SimpleAgent
    ├── Azure.AI.Projects (2.0.0)
    ├── Azure.Identity (1.21.0)
    ├── Microsoft.Agents.AI.Foundry (1.1.0)
    └── Spectre.Console (0.55.0)

ASE.SimpleAgentSearch
    ├── Azure.AI.OpenAI (latest)
    ├── Azure.Identity (1.21.0)
    ├── Microsoft.Agents.AI (latest)
    ├── Microsoft.Extensions.AI (latest)
    ├── Spectre.Console (0.55.0)
    └── ASE.Libraries (project reference)

ASE.Libraries
    └── (no external dependencies)

ASE.Libraries.Tests
    ├── Microsoft.NET.Test.Sdk (17.13.0)
    ├── xUnit (2.9.3)
    ├── xunit.runner.visualstudio (3.0.0)
    ├── coverlet.collector (6.0.3)
    └── ASE.Libraries (project reference)
```

---

## 📚 Additional Resources

- 📖 [Azure AI Projects SDK](https://learn.microsoft.com/dotnet/api/overview/azure/ai.projects.agents-readme?view=azure-dotnet)
- 📖 [Microsoft Agent Framework](https://learn.microsoft.com/agent-framework/overview/agent-framework-overview)
- 📖 [RAG Patterns](https://learn.microsoft.com/azure/ai-services/openai/concepts/use-your-data)
- 📖 [Spectre.Console Documentation](https://spectreconsole.net/)

---

*Build powerful AI agents step by step! 🚀*
