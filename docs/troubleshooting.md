# 🚨 Troubleshooting Guide

Common issues and their solutions for the Azure AI Agents project.

---

## 🔐 Authentication Issues

### ❌ DefaultAzureCredential Failed to Retrieve Token

**Error Message:**
```
Azure.Identity.CredentialUnavailableException: DefaultAzureCredential failed to retrieve a token from the included credentials.
```

**Solutions:**

1. **Verify Azure CLI Login**
   ```bash
   az login
   az account show
   ```

2. **Check Subscription**
   ```bash
   az account list --output table
   az account set --subscription "Your-Subscription-Name"
   ```

3. **Verify Resource Access**
   ```bash
   az role assignment list --assignee your-email@domain.com
   ```

4. **Clear Credential Cache**
   ```bash
   # Windows
   az account clear
   az login

   # Clear Visual Studio cache
   Remove-Item -Recurse -Force "$env:USERPROFILE\.IdentityService"
   ```

📖 **Documentation:** [DefaultAzureCredential Troubleshooting](https://learn.microsoft.com/dotnet/api/overview/azure/identity-readme#troubleshooting)

---

### ❌ Unauthorized Access to AI Resource

**Error Message:**
```
Status: 401 (Unauthorized)
```

**Solutions:**

1. **Verify RBAC Role Assignment**
   
   You need one of these roles:
   - `Cognitive Services User`
   - `Cognitive Services OpenAI User`
   - `Cognitive Services Contributor`

   ```bash
   # Assign role
   az role assignment create \
     --role "Cognitive Services User" \
     --assignee your-email@domain.com \
     --scope /subscriptions/{subscription-id}/resourceGroups/{resource-group}/providers/Microsoft.CognitiveServices/accounts/{resource-name}
   ```

2. **Check Network Access**
   - Verify firewall rules in Azure AI Foundry
   - Ensure your IP is allowed
   - Check if VNet restrictions apply

📖 **Documentation:** [Azure RBAC](https://learn.microsoft.com/azure/role-based-access-control/)

---

## ⚙️ Configuration Issues

### ❌ Environment Variable Not Set

**Error Message:**
```
ArgumentException: Please set the ENDPOINT environment variable.
```

**Solutions:**

1. **Set Variables**
   ```powershell
   # PowerShell
   $env:ENDPOINT = "https://your-project.services.ai.azure.com"
   $env:DEPLOYMENTNAME = "gpt-4o"
   ```

2. **Verify Variables**
   ```powershell
   echo $env:ENDPOINT
   echo $env:DEPLOYMENTNAME
   ```

3. **Restart IDE/Terminal**
   Environment variables may require restarting your development environment.

---

### ❌ Invalid Endpoint Format

**Error Message:**
```
UriFormatException: Invalid URI
```

**Solution:**

Ensure endpoint includes the full URL:
```
✅ CORRECT: https://your-project.services.ai.azure.com
❌ WRONG: your-project.services.ai.azure.com
❌ WRONG: https://your-project.openai.azure.com
```

---

### ❌ Model Deployment Not Found

**Error Message:**
```
The API deployment for this resource does not exist.
```

**Solutions:**

1. **Verify Deployment Name**
   ```bash
   az cognitiveservices account deployment list \
     --name your-ai-resource \
     --resource-group your-resource-group
   ```

2. **Check Deployment Status**
   - Open Azure AI Foundry
   - Navigate to **Deployments**
   - Ensure status is "Succeeded" not "Creating"

3. **Verify Exact Name**
   - Deployment names are case-sensitive
   - No extra spaces or characters

---

## 🔨 Build & Compilation Issues

### ❌ .NET 10 SDK Not Found

**Error Message:**
```
NETSDK1045: The current .NET SDK does not support targeting .NET 10.0
```

**Solution:**

1. **Install .NET 10 SDK**
   - Download: [https://dotnet.microsoft.com/download/dotnet/10.0](https://dotnet.microsoft.com/download/dotnet/10.0)

2. **Verify Installation**
   ```bash
   dotnet --version
   dotnet --list-sdks
   ```

3. **Restart IDE**

---

### ❌ Package Restore Failure

**Error Message:**
```
NU1101: Unable to find package Azure.AI.Projects
```

**Solutions:**

1. **Clear NuGet Cache**
   ```bash
   dotnet nuget locals all --clear
   dotnet restore
   ```

2. **Add Package Source**
   ```bash
   dotnet nuget add source https://api.nuget.org/v3/index.json
   ```

3. **Check Internet Connection**
   Ensure you can reach nuget.org

---

### ❌ Build Warnings About Preview SDK

**Warning Message:**
```
NETSDK1057: You are using a preview version of .NET
```

**Solution:**

This is informational only. .NET 10 is in preview. To suppress:

```xml
<PropertyGroup>
    <SuppressNETCoreSdkPreviewMessage>true</SuppressNETCoreSdkPreviewMessage>
</PropertyGroup>
```

---

## 🧪 Test Issues

### ❌ Tests Fail to Run

**Error Message:**
```
No test is available in...
```

**Solutions:**

1. **Rebuild Solution**
   ```bash
   dotnet clean
   dotnet build
   dotnet test
   ```

2. **Verify Test SDK**
   Ensure `Microsoft.NET.Test.Sdk` is installed:
   ```xml
   <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.13.0" />
   ```

3. **Check Test Discovery**
   Tests must be public with `[Fact]` or `[Theory]` attributes

---

### ❌ Test Coverage Not Generated

**Solution:**

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./coverage/
```

---

## 🚀 Runtime Issues

### ❌ Agent Response Times Out

**Error Message:**
```
TaskCanceledException: The request was canceled due to the configured HttpClient.Timeout
```

**Solutions:**

1. **Increase Timeout**
   ```csharp
   var httpClient = new HttpClient
   {
       Timeout = TimeSpan.FromMinutes(2)
   };
   ```

2. **Check Model Availability**
   - High load can cause delays
   - Try a different deployment/region

3. **Optimize Prompt**
   - Reduce input token count
   - Set `MaxTokens` limit

---

### ❌ Rate Limit Exceeded

**Error Message:**
```
Status: 429 (Too Many Requests)
```

**Solutions:**

1. **Implement Retry Logic**
   Azure SDK has built-in retries, but you can adjust:
   ```csharp
   var options = new AIProjectClientOptions();
   options.Retry.MaxRetries = 5;
   options.Retry.Delay = TimeSpan.FromSeconds(2);
   ```

2. **Check Quota**
   ```bash
   az cognitiveservices account list-usage \
     --name your-ai-resource \
     --resource-group your-resource-group
   ```

3. **Scale Up Tier**
   - Upgrade to higher pricing tier
   - Request quota increase via Azure Portal

📖 **Documentation:** [Rate Limits](https://learn.microsoft.com/azure/ai-services/openai/quotas-limits)

---

### ❌ Out of Memory Exception

**Error Message:**
```
OutOfMemoryException
```

**Solutions:**

1. **Limit Token Usage**
   ```csharp
   ChatOptions = new()
   {
       MaxTokens = 500  // Reduce from default
   }
   ```

2. **Stream Responses**
   ```csharp
   await foreach (var chunk in agent.RunStreamingAsync(query))
   {
       Console.Write(chunk);
   }
   ```

---

## 🐛 Application-Specific Issues

### ❌ Spectre.Console Display Issues

**Problem:** Text formatting not displayed correctly

**Solutions:**

1. **Check Console Support**
   ```csharp
   if (!AnsiConsole.Profile.Capabilities.Ansi)
   {
       // Fallback to plain text
       Console.WriteLine(message);
   }
   ```

2. **Use Windows Terminal**
   - Install [Windows Terminal](https://aka.ms/terminal)
   - Better ANSI support than CMD

---

### ❌ Search Returns No Results

**Problem:** `DocumentSearchAdapter.Search()` returns empty

**Solution:**

Current implementation only matches "return" and "refund" keywords. To extend:

```csharp
public static IEnumerable<SearchResult> Search(string query)
{
    ArgumentNullException.ThrowIfNull(query);

    // Add your custom keywords
    if (query.Contains("shipping", StringComparison.OrdinalIgnoreCase))
    {
        yield return new SearchResult { /* ... */ };
    }

    // Add Azure AI Search integration here
    // or replace with real search backend
}
```

---

## 📊 Performance Issues

### ❌ Slow Response Times

**Possible Causes & Solutions:**

1. **Large Context Windows**
   - Reduce input length
   - Limit search results

2. **Cold Start Latency**
   - First request is always slower
   - Keep connection alive

3. **Model Choice**
   - `gpt-4o-mini` is faster than `gpt-4o`
   - `gpt-35-turbo` is fastest

4. **Network Latency**
   - Choose Azure region closest to you
   - Use Azure VNet for production

---

## 🔍 Debugging Tips

### Enable Detailed Logging

```csharp
using Azure.Core.Diagnostics;

// Console logging
using var listener = AzureEventSourceListener.CreateConsoleLogger(EventLevel.Verbose);

// File logging
using var listener = AzureEventSourceListener.CreateTraceLogger(EventLevel.Verbose);
```

### Inspect HTTP Traffic

```csharp
var options = new AIProjectClientOptions();
options.Diagnostics.IsLoggingContentEnabled = true;
options.Diagnostics.IsLoggingEnabled = true;

var client = new AIProjectClient(new Uri(endpoint), credential, options);
```

### Use Fiddler/Wireshark
Capture network traffic to diagnose API issues.

---

## 🆘 Getting Additional Help

### Check Logs
```bash
# Azure Activity Log
az monitor activity-log list --resource-group your-resource-group

# Application Insights (if configured)
az monitor app-insights query --app your-app --analytics-query "traces | top 50 by timestamp desc"
```

### Support Channels

- 📧 [GitHub Issues](https://github.com/bovrhovn/azure-demos-agents-from-scratch-to-enterprise/issues)
- 💬 [GitHub Discussions](https://github.com/bovrhovn/azure-demos-agents-from-scratch-to-enterprise/discussions)
- 📖 [Microsoft Q&A](https://learn.microsoft.com/answers/tags/133/azure-openai/)
- 🎫 [Azure Support](https://azure.microsoft.com/support/options/)

---

## 📚 Useful Commands

### Quick Diagnostics

```bash
# System info
dotnet --info
az --version

# Azure account
az account show
az account list --output table

# Resource status
az resource show --ids /subscriptions/{sub}/resourceGroups/{rg}/providers/Microsoft.CognitiveServices/accounts/{name}

# Network test
Test-NetConnection your-project.services.ai.azure.com -Port 443

# Environment check
echo $env:ENDPOINT
echo $env:DEPLOYMENTNAME
```

---

## 🔗 Additional Resources

- 📖 [Azure AI Services Troubleshooting](https://learn.microsoft.com/azure/ai-services/openai/troubleshooting)
- 📖 [Azure SDK for .NET Issues](https://github.com/Azure/azure-sdk-for-net/issues)
- 📖 [Stack Overflow - Azure Tag](https://stackoverflow.com/questions/tagged/azure)

---

*When in doubt, check the logs! 🔍🐛*
