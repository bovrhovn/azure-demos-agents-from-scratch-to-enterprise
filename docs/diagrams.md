# 📊 Architecture & Flow Diagrams

Visual documentation of the Azure AI Agents solution — project structure, data flows, class relationships, and test coverage.

---

## 📁 Repository Overview

```mermaid
graph TD
    ROOT["🏠 azure-demos-agents-from-scratch-to-enterprise"]

    ROOT --> SRC["📁 src/"]
    ROOT --> TESTS["📁 tests/"]
    ROOT --> DOCS["📁 docs/"]
    ROOT --> SCRIPTS["📁 scripts/"]

    SRC --> ASE["📁 AgentScratchEnterprise/"]
    SRC --> WEBAPP["📁 chat-web-app/ (Vue 3 + TypeScript)"]

    ASE --> SIMPLE["ASE.SimpleAgent\n(Basic Q&A Agent)"]
    ASE --> SEARCH["ASE.SimpleAgentSearch\n(RAG Agent)"]
    ASE --> LIBS["ASE.Libraries\n(Shared Components)"]
    ASE --> EAPI["ASE.EnterpriseApi\n(ASP.NET Minimal API)"]

    TESTS --> LIBTESTS["ASE.Libraries.Tests\n(51 unit tests)"]

    LIBS -.->|"project ref"| SEARCH
    LIBS -.->|"project ref"| EAPI
    LIBS -.->|"project ref"| LIBTESTS

    style ROOT fill:#0078D4,color:#fff
    style ASE fill:#512BD4,color:#fff
    style WEBAPP fill:#41B883,color:#fff
    style LIBTESTS fill:#2EA043,color:#fff
```

---

## 🏗 Solution Architecture

```mermaid
flowchart TD
    subgraph AZURE["☁️ Azure Cloud"]
        AAF["Azure AI Foundry\nProject"]
        AOI["Azure OpenAI\n(GPT-4o / GPT-4o-mini)"]
        AIS["Azure AI Search\n(Roadmap)"]
        APPINS["Application Insights\n(Telemetry)"]
        AAF <--> AOI
    end

    subgraph DOTNET[".NET 10 Applications"]
        SA["🤖 ASE.SimpleAgent\n(Console App)"]
        SAS["🔍 ASE.SimpleAgentSearch\n(Console App)"]
        EAPI["🌐 ASE.EnterpriseApi\n(ASP.NET Minimal API)"]
        LIBS["📦 ASE.Libraries\n(Shared)"]

        SAS --> LIBS
        EAPI --> LIBS
    end

    subgraph FRONTEND["Frontend"]
        VUE["⚛️ chat-web-app\n(Vue 3 + TypeScript)"]
    end

    SA -->|"DefaultAzureCredential\nHTTPS"| AAF
    SAS -->|"DefaultAzureCredential\nHTTPS"| AOI
    SAS -->|"OpenTelemetry"| APPINS
    EAPI -->|"REST API"| AOI
    EAPI -.->|"(roadmap)"| AIS
    VUE -->|"CORS / REST"| EAPI
```

---

## 🤖 SimpleAgent — Data Flow

```mermaid
sequenceDiagram
    actor User
    participant Console as Spectre.Console
    participant Agent as AIAgent
    participant Client as AIProjectClient
    participant Azure as Azure AI Projects API
    participant OpenAI as Azure OpenAI

    User->>Console: Enter question
    Console->>Agent: RunAsync(question)
    Agent->>Client: Create agent session
    Client->>Azure: POST /agents/runs
    Azure->>OpenAI: Forward prompt + instructions
    OpenAI-->>Azure: GPT response
    Azure-->>Client: RunResult
    Client-->>Agent: answer (string)
    Agent-->>Console: Display answer
    Console-->>User: [green]Answer:[/] ...
```

---

## 🔍 SimpleAgentSearch — RAG Flow

```mermaid
sequenceDiagram
    actor User
    participant Console as Spectre.Console
    participant Agent as AIAgent (RAG)
    participant TSP as TextSearchProvider
    participant DSA as DocumentSearchAdapter
    participant OTel as OpenTelemetry / AppInsights
    participant OpenAI as Azure OpenAI

    User->>Console: Enter query
    Console->>Agent: RunAsync(question)

    Note over Agent,TSP: SearchTime = BeforeAIInvoke

    Agent->>TSP: GetContextAsync(query)
    TSP->>DSA: Search(query)
    DSA-->>TSP: IEnumerable<SearchResult>
    Note over DSA: Keyword matching:\n"return", "refund", "amount"
    TSP-->>Agent: TextSearchResult[] (SourceName, SourceLink, Text)

    Agent->>OpenAI: Chat completion (query + context documents)
    OpenAI-->>Agent: Response with citations
    Agent->>OTel: Export traces + metrics
    Agent-->>Console: AgentResponse.Text
    Console-->>User: Answer + cited sources
```

---

## 🌐 EnterpriseApi — Architecture & Request Flow

```mermaid
flowchart LR
    subgraph CLIENT["Client"]
        VUE["⚛️ Vue 3 App\n(chat-web-app)"]
    end

    subgraph API["ASE.EnterpriseApi (ASP.NET)"]
        CORS["CORS Policy\n(AllowVueApp)"]
        ROUTES["Route Group\n/basic/*"]
        CACHE["IMemoryCache"]
        HEALTH["Health Check\n/health"]
        EXH["Exception Handler"]

        subgraph ENDPOINTS["Endpoints"]
            GET["GET /basic/get-all\nLoadDataAsync()"]
            SEARCH["GET /basic/search?query=\nSearchAsync()"]
        end

        ROUTES --> ENDPOINTS
    end

    subgraph LIBS["ASE.Libraries"]
        DSA["DocumentSearchAdapter\n(LOCAL mode)"]
        AZSA["AzureSearchDocumentSearchAdapter\n(AZURE mode)"]
        BDG["BankDataGenerator\n(Bogus)"]
        ISS["ISearchService"]

        ISS <|.. DSA
        ISS <|.. AZSA
    end

    VUE -->|"HTTP GET"| CORS
    CORS --> ROUTES
    GET --> BDG
    SEARCH -->|"inject ISearchService"| ISS
    ISS --> DSA
    ISS --> AZSA
    CACHE -.->|"(future)"| SEARCH
```

---

## 🌐 EnterpriseApi — Request Sequence

```mermaid
sequenceDiagram
    participant VUE as Vue App
    participant API as ASP.NET API
    participant Cache as IMemoryCache
    participant Search as ISearchService
    participant BDG as BankDataGenerator

    VUE->>API: GET /basic/get-all
    API->>BDG: GenerateBankDataList(10)
    BDG-->>API: List<BankData>
    API-->>VUE: 200 OK [BankData]

    VUE->>API: GET /basic/search?query=return
    API->>Search: Search("return")
    Search-->>API: IEnumerable<SearchResult>
    API-->>VUE: 200 OK [SearchResult]
```

---

## 📦 ASE.Libraries — Class Diagram

```mermaid
classDiagram
    class ISearchService {
        <<interface>>
        +Search(query: string) IEnumerable~SearchResult~
        +Search(query: string, recordCount: int) IEnumerable~SearchResult~
    }

    class DocumentSearchAdapter {
        +Search(query: string) IEnumerable~SearchResult~
        +Search(query: string, recordCount: int) IEnumerable~SearchResult~
    }

    class AzureSearchDocumentSearchAdapter {
        +Search(query: string) IEnumerable~SearchResult~
        +Search(query: string, recordCount: int) IEnumerable~SearchResult~
    }

    class SearchResult {
        <<sealed>>
        +SourceName: string
        +SourceLink: string
        +Text: string
    }

    class BankDataGenerator {
        <<static>>
        +GenerateCard() BankCard
        +GenerateCards(count: int) List~BankCard~
        +GenerateTransaction(card: BankCard) BankTransaction
        +GenerateTransactions(card: BankCard, count: int) List~BankTransaction~
        +GenerateStatement(card: BankCard) BankStatement
        +GenerateBankData() BankData
        +GenerateBankDataList(count: int) List~BankData~
    }

    class BankCard {
        +CardNumber: string
        +CardType: string
        +CardHolderName: string
        +CVV: string
        +ExpirationDate: DateTime
    }

    class BankTransaction {
        +TransactionId: string
        +Amount: decimal
        +Description: string
        +Date: DateTime
        +Card: BankCard
    }

    class BankStatement {
        +Card: BankCard
        +Transactions: List~BankTransaction~
        +Balance: decimal
    }

    class BankData {
        +Cards: List~BankCard~
        +Balance: decimal
    }

    class RouteNames {
        <<static>>
        +BasicRoute: string = "basic"
        +GetRoute: string = "get-all"
        +SearchRoute: string = "search"
    }

    ISearchService <|.. DocumentSearchAdapter
    ISearchService <|.. AzureSearchDocumentSearchAdapter
    DocumentSearchAdapter ..> SearchResult : returns
    AzureSearchDocumentSearchAdapter ..> SearchResult : returns
    BankDataGenerator ..> BankCard : creates
    BankDataGenerator ..> BankTransaction : creates
    BankDataGenerator ..> BankStatement : creates
    BankDataGenerator ..> BankData : creates
    BankStatement --> BankCard
    BankStatement --> BankTransaction
    BankTransaction --> BankCard
    BankData --> BankCard
```

---

## 🧪 Test Coverage — Distribution

```mermaid
pie title Test Distribution (51 total)
    "BankDataGeneratorTests" : 18
    "DocumentSearchAdapterTests" : 8
    "RouteNamesTests" : 7
    "BankModelsTests" : 6
    "SearchResultTests" : 4
    "AzureSearchDocumentSearchAdapterTests" : 4
    "ISearchServiceTests" : 4
```

---

## 🧪 Test Architecture

```mermaid
graph LR
    subgraph TESTPROJ["tests/ASE.Libraries.Tests"]
        T1["BankDataGeneratorTests\n18 tests"]
        T2["DocumentSearchAdapterTests\n8 tests"]
        T3["RouteNamesTests\n7 tests"]
        T4["BankModelsTests\n6 tests"]
        T5["SearchResultTests\n4 tests"]
        T6["AzureSearchDocumentSearchAdapterTests\n4 tests"]
        T7["ISearchServiceTests\n4 tests"]
    end

    subgraph LIBPROJ["src/ASE.Libraries (under test)"]
        BDG["BankDataGenerator"]
        DSA["DocumentSearchAdapter"]
        AZSA["AzureSearchDocumentSearchAdapter"]
        RN["RouteNames"]
        BM["BankCard\nBankTransaction\nBankStatement\nBankData"]
        SR["SearchResult"]
        ISS["ISearchService"]
    end

    T1 -->|tests| BDG
    T2 -->|tests| DSA
    T3 -->|tests| RN
    T4 -->|tests| BM
    T5 -->|tests| SR
    T6 -->|tests| AZSA
    T7 -->|tests| ISS
```

---

## 🔐 Authentication Chain

```mermaid
flowchart TD
    APP["Application\n(SimpleAgent / SimpleAgentSearch)"]
    DAC["DefaultAzureCredential"]

    APP --> DAC

    DAC --> ENV["1️⃣ Environment Variables\nAZURE_CLIENT_ID\nAZURE_TENANT_ID\nAZURE_CLIENT_SECRET"]
    ENV -->|"not found"| MI["2️⃣ Managed Identity\n(Azure-hosted resources)"]
    MI -->|"not found"| VS["3️⃣ Visual Studio\nSigned-in account"]
    VS -->|"not found"| VSCODE["4️⃣ VS Code\nAzure Account extension"]
    VSCODE -->|"not found"| CLI["5️⃣ Azure CLI\naz login"]
    CLI -->|"not found"| APS["6️⃣ Azure PowerShell\nConnect-AzAccount"]

    ENV -->|"✅ found"| TOKEN["Azure Access Token"]
    MI -->|"✅ found"| TOKEN
    VS -->|"✅ found"| TOKEN
    VSCODE -->|"✅ found"| TOKEN
    CLI -->|"✅ found"| TOKEN
    APS -->|"✅ found"| TOKEN

    TOKEN --> AZURE["Azure AI Foundry\n/ Azure OpenAI"]
```

---

## 📦 Project Dependencies

```mermaid
graph LR
    subgraph NUGET["NuGet Packages"]
        AIP["Azure.AI.Projects\n2.0.0"]
        AID["Azure.Identity\n1.21.0"]
        MAF["Microsoft.Agents.AI.Foundry\n1.1.0"]
        SC["Spectre.Console\n0.55.0"]
        AOAI["Azure.AI.OpenAI"]
        MAI["Microsoft.Agents.AI"]
        MEAI["Microsoft.Extensions.AI"]
        MCP["ModelContextProtocol.Core\n1.2.0"]
        OT["OpenTelemetry + Azure Monitor"]
        XUNIT["xUnit 2.9.3"]
        BOGUS["Bogus 35.6.5"]
        TESTSDK["Microsoft.NET.Test.Sdk"]
        COV["coverlet.collector"]
    end

    subgraph PROJECTS["Solution Projects"]
        SA["ASE.SimpleAgent"]
        SAS["ASE.SimpleAgentSearch"]
        LIBS["ASE.Libraries"]
        EAPI["ASE.EnterpriseApi"]
        LT["ASE.Libraries.Tests"]
    end

    SA --> AIP
    SA --> AID
    SA --> MAF
    SA --> SC
    SA --> MCP

    SAS --> AOAI
    SAS --> AID
    SAS --> MAI
    SAS --> MEAI
    SAS --> SC
    SAS --> OT
    SAS --> LIBS

    EAPI --> LIBS

    LT --> LIBS
    LT --> XUNIT
    LT --> BOGUS
    LT --> TESTSDK
    LT --> COV
```

---

## 🗺 Progressive Enhancement Roadmap

```mermaid
graph LR
    P1["Phase 1 ✅\nASE.SimpleAgent\nBasic Q&A"]
    P2["Phase 2 ✅\nASE.SimpleAgentSearch\nRAG + Search"]
    P3["Phase 3 🔄\nVector Search\n(Azure AI Search)"]
    P4["Phase 4 🔄\nObservability\n(App Insights)"]
    P5["Phase 5 🔄\nEnterprise Security\n(EntraID + RBAC)"]

    P1 -->|"+ TextSearchProvider\n+ DocumentSearchAdapter"| P2
    P2 -->|"+ AzureSearchDocumentSearchAdapter\n+ Embeddings"| P3
    P3 -->|"+ OpenTelemetry\n+ Distributed Tracing"| P4
    P4 -->|"+ ManagedIdentity\n+ RBAC\n+ Audit Logging"| P5

    style P1 fill:#2EA043,color:#fff
    style P2 fill:#2EA043,color:#fff
    style P3 fill:#E3A008,color:#fff
    style P4 fill:#E3A008,color:#fff
    style P5 fill:#E3A008,color:#fff
```

---

## 🌐 Chat Web App — Component Structure

```mermaid
graph TD
    subgraph VUE["chat-web-app (Vue 3 + TypeScript)"]
        MAIN["main.ts\n(App bootstrap)"]
        APP["App.vue\n(Root component)"]
        ROUTER["router/\n(Vue Router)"]
        SV["SearchView.vue\n(Search page)"]
        SS["searchService.ts\n(API client)"]
        TYPES["types/\n(TypeScript interfaces)"]
        COMP["components/\n(Reusable UI)"]
        COMP2["composables/\n(Composition API hooks)"]

        MAIN --> APP
        APP --> ROUTER
        ROUTER --> SV
        SV --> SS
        SS --> TYPES
        SV --> COMP
        SV --> COMP2
    end

    subgraph ENV["Environment"]
        VITE_ENV["VITE_API_BASE_URL\nhttps://localhost:5066"]
    end

    SS -->|"GET /basic/search?query="| VITE_ENV
    SS -->|"GET /basic/get-all"| VITE_ENV
```

---

*Diagrams auto-rendered by GitHub and most Markdown viewers supporting Mermaid.*
