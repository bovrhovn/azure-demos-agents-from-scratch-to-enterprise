# ⚙️ Configuration Guide

This guide covers all configuration options, environment variables, and settings for the Azure AI Agents project.

---

## 🔐 Environment Variables

### Required Configuration

These environment variables **must** be set before running the applications:

#### `ENDPOINT`
**Description:** Azure AI Foundry project endpoint URL

**Format:** `https://<resource-name>.services.ai.azure.com`

**Example:**
```bash
https://my-ai-project.services.ai.azure.com
```

**How to find:**
1. Navigate to [Azure AI Foundry](https://ai.azure.com/)
2. Open your project
3. Go to **Settings** → **Project properties**
4. Copy the **Project endpoint**

📖 **Documentation:** [Azure AI Foundry Projects](https://learn.microsoft.com/azure/ai-foundry/how-to/create-projects)

---

#### `DEPLOYMENTNAME`
**Description:** Name of your deployed AI model

**Examples:**
- `gpt-4o`
- `gpt-4o-mini`
- `gpt-4-turbo`
- `gpt-35-turbo`

**How to find:**
1. In Azure AI Foundry, open your project
2. Navigate to **Deployments**
3. Copy the **Deployment name** column value

📖 **Documentation:** [Deploy Azure OpenAI Models](https://learn.microsoft.com/azure/ai-services/openai/how-to/create-resource)

---

### Setting Environment Variables

#### Windows PowerShell
```powershell
# Set for current session
$env:ENDPOINT = "https://your-project.services.ai.azure.com"
$env:DEPLOYMENTNAME = "gpt-4o"

# Verify
echo $env:ENDPOINT
echo $env:DEPLOYMENTNAME

# Set permanently (user level)
[System.Environment]::SetEnvironmentVariable("ENDPOINT", "https://your-project.services.ai.azure.com", "User")
[System.Environment]::SetEnvironmentVariable("DEPLOYMENTNAME", "gpt-4o", "User")
```

#### Windows Command Prompt
```cmd
set ENDPOINT=https://your-project.services.ai.azure.com
set DEPLOYMENTNAME=gpt-4o
```

#### Linux/macOS Bash
```bash
export ENDPOINT="https://your-project.services.ai.azure.com"
export DEPLOYMENTNAME="gpt-4o"

# Add to ~/.bashrc or ~/.zshrc for persistence
echo 'export ENDPOINT="https://your-project.services.ai.azure.com"' >> ~/.bashrc
echo 'export DEPLOYMENTNAME="gpt-4o"' >> ~/.bashrc
```

---

## 🔒 Azure Authentication Configuration

### Development Environment

The applications use `DefaultAzureCredential` which attempts authentication in this order:

1. **Environment variables** (see [Service Principal Configuration](#service-principal-configuration))
2. **Managed Identity** (when running on Azure resources)
3. **Visual Studio / VS Code** (signed-in account)
4. **Azure CLI** (`az login`)
5. **Azure PowerShell** (`Connect-AzAccount`)

### Azure CLI Authentication (Recommended for Development)

```bash
# Login to Azure
az login

# Verify authentication
az account show

# Set default subscription (if you have multiple)
az account set --subscription "Your-Subscription-Name"
```

📖 **Documentation:** [Azure CLI Authentication](https://learn.microsoft.com/cli/azure/authenticate-azure-cli)

---

### Service Principal Configuration

For automated scenarios (CI/CD, production), use a service principal:

#### Create Service Principal
```bash
az ad sp create-for-rbac --name "MyAIAgentApp" --role "Cognitive Services User" --scopes /subscriptions/{subscription-id}/resourceGroups/{resource-group}/providers/Microsoft.CognitiveServices/accounts/{resource-name}
```

#### Configure Environment Variables
```bash
# Windows PowerShell
$env:AZURE_CLIENT_ID = "your-client-id"
$env:AZURE_TENANT_ID = "your-tenant-id"
$env:AZURE_CLIENT_SECRET = "your-client-secret"

# Linux/macOS
export AZURE_CLIENT_ID="your-client-id"
export AZURE_TENANT_ID="your-tenant-id"
export AZURE_CLIENT_SECRET="your-client-secret"
```

⚠️ **Security Warning:** Never commit credentials to source control!

📖 **Documentation:** [Azure Service Principal](https://learn.microsoft.com/azure/active-directory/develop/howto-create-service-principal-portal)

---

### Managed Identity (Production Recommended)

When running on Azure resources (App Service, Container Apps, VMs), use Managed Identity:

```csharp
// Use ManagedIdentityCredential instead of DefaultAzureCredential
var credential = new ManagedIdentityCredential();
AIAgent agent = new AIProjectClient(new Uri(endpoint), credential).AsAIAgent(...);
```

**Benefits:**
- ✅ No credentials to manage
- ✅ Automatic credential rotation
- ✅ Reduced security risk
- ✅ Azure manages authentication

**Setup:**
1. Enable Managed Identity on your Azure resource
2. Grant "Cognitive Services User" role to the managed identity
3. Deploy your application

📖 **Documentation:** [Managed Identity](https://learn.microsoft.com/azure/active-directory/managed-identities-azure-resources/)

---

## ⚙️ Application Configuration

### ASE.SimpleAgent Configuration

**Agent Instructions:**
```csharp
instructions: "You are a friendly assistant. Keep your answers brief."
```

Customize the agent's behavior by modifying the `instructions` parameter:

```csharp
AIAgent agent = new AIProjectClient(new Uri(endpoint), new DefaultAzureCredential())
    .AsAIAgent(
        model: deploymentName,
        instructions: "You are an expert Azure architect. Provide detailed technical explanations.",
        name: "AzureExpert"
    );
```

**Configuration Options:**
- `model` - Deployment name
- `instructions` - System prompt defining agent behavior
- `name` - Agent identifier (for logging/tracking)

---

### ASE.SimpleAgentSearch Configuration

**Search Behavior:**
```csharp
TextSearchProviderOptions textSearchOptions = new()
{
    SearchTime = TextSearchProviderOptions.TextSearchBehavior.BeforeAIInvoke,
};
```

**Available Search Times:**
- `BeforeAIInvoke` - Search documents before generating response (default)
- `AfterAIInvoke` - Generate response first, then search for validation
- `None` - Disable search

**Chat Options:**
```csharp
ChatOptions = new()
{
    Instructions = "You are a helpful support specialist. Answer questions using the provided context and cite the source document when available."
}
```

Customize instructions to match your use case:
- Customer support
- Technical documentation
- Policy inquiries
- Knowledge base queries

---

## 🎨 UI Configuration

### Spectre.Console Settings

**Question Prompt Customization:**
```csharp
var question = AnsiConsole.Ask<string>(
    "Ask your question",
    "Default question here"  // Default value
);
```

**Color Schemes:**
```csharp
AnsiConsole.MarkupLine("[green]Question:[/]" + question);
AnsiConsole.MarkupLine("[blue]Answer:[/]" + answer);
AnsiConsole.MarkupLine("[red]Error:[/]" + error);
```

**Available Colors:**
- `green`, `blue`, `red`, `yellow`, `cyan`, `magenta`
- `bold`, `italic`, `underline`
- Combine: `[bold green]Text[/]`

📖 **Documentation:** [Spectre.Console](https://spectreconsole.net/)

---

## 🔧 Advanced Configuration

### Model-Specific Parameters

For more control over AI responses, configure ChatOptions:

```csharp
ChatOptions = new()
{
    Temperature = 0.7f,           // Creativity (0.0 - 1.0)
    MaxTokens = 500,              // Max response length
    TopP = 0.95f,                 // Nucleus sampling
    FrequencyPenalty = 0.0f,      // Reduce repetition
    PresencePenalty = 0.0f,       // Encourage topic diversity
}
```

**Parameter Guidelines:**

| Parameter | Low Value (0.0-0.3) | Medium (0.4-0.7) | High (0.8-1.0) |
|-----------|---------------------|------------------|----------------|
| **Temperature** | Focused, deterministic | Balanced | Creative, varied |
| **TopP** | Precise | Balanced | Diverse |

📖 **Documentation:** [Azure OpenAI Parameters](https://learn.microsoft.com/azure/ai-services/openai/how-to/chatgpt)

---

### Timeout Configuration

```csharp
// Configure HTTP client timeout
var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromSeconds(120)
};
```

---

### Retry Policy

```csharp
// Configure retry behavior (built into Azure SDK)
var clientOptions = new AIProjectClientOptions
{
    Retry =
    {
        MaxRetries = 3,
        Delay = TimeSpan.FromSeconds(1),
        MaxDelay = TimeSpan.FromSeconds(30),
        Mode = RetryMode.Exponential
    }
};

var client = new AIProjectClient(new Uri(endpoint), credential, clientOptions);
```

---

## 📊 Logging Configuration

### Enable Console Logging

```csharp
using Azure.Core.Diagnostics;

// Enable Azure SDK logging
using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();
```

### Application Insights Integration

For production monitoring:

```csharp
services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = "your-connection-string";
});
```

📖 **Documentation:** [Application Insights](https://learn.microsoft.com/azure/azure-monitor/app/app-insights-overview)

---

## 🐳 Docker Configuration

### Environment File (.env)

```env
ENDPOINT=https://your-project.services.ai.azure.com
DEPLOYMENTNAME=gpt-4o
AZURE_CLIENT_ID=your-client-id
AZURE_TENANT_ID=your-tenant-id
AZURE_CLIENT_SECRET=your-client-secret
```

### Docker Compose

```yaml
version: '3.8'
services:
  simpleagent:
    image: ase-simpleagent:latest
    env_file:
      - .env
    environment:
      - ENDPOINT=${ENDPOINT}
      - DEPLOYMENTNAME=${DEPLOYMENTNAME}
```

---

## 🔍 Configuration Validation

Use this script to verify your configuration:

```powershell
# validate-config.ps1
$errors = @()

if (-not $env:ENDPOINT) {
    $errors += "ENDPOINT environment variable not set"
}

if (-not $env:DEPLOYMENTNAME) {
    $errors += "DEPLOYMENTNAME environment variable not set"
}

# Test Azure authentication
try {
    az account show | Out-Null
} catch {
    $errors += "Azure CLI authentication failed. Run 'az login'"
}

# Test endpoint connectivity
try {
    $response = Invoke-WebRequest -Uri $env:ENDPOINT -Method Head -TimeoutSec 5
} catch {
    $errors += "Cannot reach endpoint: $env:ENDPOINT"
}

if ($errors.Count -eq 0) {
    Write-Host "✅ Configuration validated successfully!" -ForegroundColor Green
} else {
    Write-Host "❌ Configuration errors found:" -ForegroundColor Red
    $errors | ForEach-Object { Write-Host "   - $_" -ForegroundColor Red }
}
```

---

## 📚 Configuration Resources

- 📖 [Azure AI Configuration](https://learn.microsoft.com/azure/ai-services/openai/how-to/create-resource)
- 📖 [Azure Identity](https://learn.microsoft.com/dotnet/api/overview/azure/identity-readme)
- 📖 [Environment Variables in .NET](https://learn.microsoft.com/dotnet/api/system.environment.getenvironmentvariable)
- 📖 [Azure Key Vault](https://learn.microsoft.com/azure/key-vault/general/overview) (for production secrets)

---

*Proper configuration ensures secure and reliable AI agents! 🔐⚙️*
