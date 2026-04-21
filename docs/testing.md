# 🧪 Testing Guide

This document covers the testing strategy, test execution, and coverage details for the Azure AI Agents project.

---

## 📋 Testing Overview

The project includes comprehensive unit tests with **100% coverage** of the `ASE.Libraries` components. Tests are written using **xUnit**, a modern testing framework for .NET.

### Test Statistics

| Metric | Value |
|--------|-------|
| **Total Tests** | 17 |
| **Passed Tests** | ✅ 17 |
| **Failed Tests** | ❌ 0 |
| **Code Coverage** | 100% (Library components) |
| **Test Framework** | xUnit 2.9.3 |
| **Test Execution Time** | ~2 seconds |

---

## 🏗 Test Project Structure

```
tests/
└── ASE.Libraries.Tests/
    ├── ASE.Libraries.Tests.csproj
    ├── DocumentSearchAdapterTests.cs
    └── GlobalUsings.cs
```

### Project Configuration

The test project targets **.NET 10.0** and includes the following packages:

```xml
<ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.13.0" />
    <PackageReference Include="xUnit" Version="2.9.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="3.0.0" />
    <PackageReference Include="coverlet.collector" Version="6.0.3" />
</ItemGroup>
```

---

## 🚀 Running Tests

### Command Line

#### Run all tests
```bash
cd tests\ASE.Libraries.Tests
dotnet test
```

#### Run with detailed output
```bash
dotnet test --logger "console;verbosity=detailed"
```

#### Run with code coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Visual Studio

1. Open the solution in Visual Studio 2026
2. Open **Test Explorer** (Test → Test Explorer)
3. Click **Run All** to execute all tests
4. View results in the Test Explorer window

### Visual Studio Code

1. Install the [C# Dev Kit extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
2. Open the Testing sidebar (flask icon)
3. Click **Run All Tests**

---

## 📝 Test Cases

### `DocumentSearchAdapterTests`

Comprehensive tests for the `DocumentSearchAdapter` class.

#### ✅ Search Functionality Tests

| Test Name | Purpose | Assertion |
|-----------|---------|-----------|
| `Search_WithReturnKeyword_ReturnsReturnPolicyResult` | Verifies "return" keyword triggers policy | Returns 1 result with correct content |
| `Search_WithRefundKeyword_ReturnsReturnPolicyResult` | Verifies "refund" keyword triggers policy | Returns 1 result with "refund" text |
| `Search_WithBothReturnAndRefundKeywords_ReturnsResult` | Tests multiple keyword matching | Returns single deduplicated result |
| `Search_WithUnrelatedQuery_ReturnsEmpty` | Ensures non-matching queries return nothing | Returns empty collection |
| `Search_WithEmptyString_ReturnsEmpty` | Handles empty input gracefully | Returns empty collection |

#### ✅ Case Sensitivity Tests

| Test Name | Purpose | Assertion |
|-----------|---------|-----------|
| `Search_WithCaseInsensitiveReturnKeyword_ReturnsReturnPolicyResult` | Uppercase "RETURN" matches | Returns 1 result |
| `Search_WithCaseInsensitiveRefundKeyword_ReturnsReturnPolicyResult` | Uppercase "REFUND" matches | Returns 1 result |
| `Search_WithVariousCasing_ReturnsResults` (Theory) | Tests all case variations | All variations return results |

**Theory Test Data:**
- "return", "Return", "RETURN"
- "refund", "Refund", "REFUND"

#### ✅ Error Handling Tests

| Test Name | Purpose | Assertion |
|-----------|---------|-----------|
| `Search_WithNullQuery_ThrowsArgumentNullException` | Validates null input handling | Throws `ArgumentNullException` |

#### ✅ Data Model Tests

| Test Name | Purpose | Assertion |
|-----------|---------|-----------|
| `SearchResult_CanBeInitialized` | Verifies object initialization | All properties set correctly |
| `SearchResult_DefaultValues_AreEmptyStrings` | Checks default property values | All strings are empty |
| `SearchResult_TextContent_ContainsExpectedInformation` | Validates result content | Contains expected policy details |

---

## 🧪 Test Examples

### Basic Search Test

```csharp
[Fact]
public void Search_WithReturnKeyword_ReturnsReturnPolicyResult()
{
    // Arrange
    const string query = "What is your return policy?";

    // Act
    var results = DocumentSearchAdapter.Search(query).ToList();

    // Assert
    Assert.NotEmpty(results);
    var result = Assert.Single(results);
    Assert.Equal("Contoso Outdoors Return Policy", result.SourceName);
    Assert.Equal("https://contoso.com/policies/returns", result.SourceLink);
    Assert.Contains("30 days", result.Text);
}
```

### Theory-Based Test (Data-Driven)

```csharp
[Theory]
[InlineData("return")]
[InlineData("Return")]
[InlineData("RETURN")]
[InlineData("refund")]
[InlineData("Refund")]
[InlineData("REFUND")]
public void Search_WithVariousCasing_ReturnsResults(string keyword)
{
    // Arrange
    var query = $"I need information about {keyword}";

    // Act
    var results = DocumentSearchAdapter.Search(query).ToList();

    // Assert
    Assert.NotEmpty(results);
}
```

### Exception Testing

```csharp
[Fact]
public void Search_WithNullQuery_ThrowsArgumentNullException()
{
    // Arrange
    string? query = null;

    // Act & Assert
    Assert.Throws<ArgumentNullException>(() => 
        DocumentSearchAdapter.Search(query!).ToList());
}
```

---

## 📊 Test Coverage

### Coverage by Component

| Component | Coverage | Lines Covered |
|-----------|----------|---------------|
| `DocumentSearchAdapter.Search()` | 100% | All branches |
| `SearchResult` (class) | 100% | All properties |

### Uncovered Scenarios

The following are **intentionally not tested** as they're application entry points:

- `ASE.SimpleAgent/Program.cs` - Console app entry point
- `ASE.SimpleAgentSearch/Program.cs` - Console app entry point

These would require integration tests with live Azure services.

---

## 🔍 Test Best Practices Applied

### 1. **Arrange-Act-Assert Pattern**
All tests follow the AAA pattern for clarity:
```csharp
// Arrange - Set up test data
const string query = "test query";

// Act - Execute the method under test
var results = DocumentSearchAdapter.Search(query);

// Assert - Verify the outcome
Assert.NotEmpty(results);
```

### 2. **Descriptive Test Names**
Test names clearly describe the scenario and expected outcome:
```
[Method]_With[Scenario]_[ExpectedBehavior]
```

Examples:
- `Search_WithReturnKeyword_ReturnsReturnPolicyResult`
- `Search_WithNullQuery_ThrowsArgumentNullException`

### 3. **Single Assertion Focus**
Each test validates one specific behavior.

### 4. **Theory Tests for Similar Cases**
Data-driven tests reduce duplication:
```csharp
[Theory]
[InlineData("return")]
[InlineData("RETURN")]
public void Search_WithVariousCasing_ReturnsResults(string keyword)
```

### 5. **Edge Case Coverage**
Tests include:
- ✅ Null input
- ✅ Empty strings
- ✅ Case sensitivity
- ✅ Multiple keywords
- ✅ No matches

---

## 🎯 Future Testing Improvements

### Integration Tests
Test real Azure AI interactions:
```csharp
[Fact]
public async Task SimpleAgent_WithRealAzureAI_ReturnsValidResponse()
{
    // Requires live Azure credentials
    // Tests end-to-end agent interaction
}
```

### Performance Tests
Measure response times:
```csharp
[Fact]
public void Search_WithLargeDataset_CompletesUnderTimeLimit()
{
    // Benchmark search performance
}
```

### Load Tests
Simulate concurrent requests:
```csharp
[Fact]
public async Task Agent_HandlesMultipleConcurrentRequests()
{
    // Test scalability
}
```

---

## 🐛 Debugging Tests

### Run a Single Test
```bash
dotnet test --filter "FullyQualifiedName~Search_WithReturnKeyword"
```

### View Detailed Output
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Debug in Visual Studio
1. Set a breakpoint in the test method
2. Right-click the test in Test Explorer
3. Select **Debug**

### Debug in VS Code
1. Set a breakpoint in the test file
2. Open the Testing sidebar
3. Right-click the test → **Debug Test**

---

## 📚 Testing Resources

### xUnit Documentation
- 📖 [xUnit.net Official Docs](https://xunit.net/)
- 📖 [xUnit Best Practices](https://xunit.net/docs/comparisons)

### .NET Testing
- 📖 [Unit Testing in .NET](https://learn.microsoft.com/dotnet/core/testing/)
- 📖 [Test-Driven Development](https://learn.microsoft.com/dotnet/core/testing/unit-testing-best-practices)

### Code Coverage
- 📖 [Coverlet Documentation](https://github.com/coverlet-coverage/coverlet)
- 📖 [Code Coverage in .NET](https://learn.microsoft.com/dotnet/core/testing/unit-testing-code-coverage)

---

## ✅ Test Checklist

Before committing code, ensure:

- ✅ All tests pass locally
- ✅ No failing tests
- ✅ New functionality has corresponding tests
- ✅ Test names follow naming conventions
- ✅ Edge cases are covered
- ✅ Tests are independent (no shared state)
- ✅ Tests run in under 5 seconds

---

## 🆘 Troubleshooting

### Tests Fail with ".NET 10 SDK Not Found"

**Solution:**
```bash
dotnet --version  # Verify .NET 10 is installed
```

### Tests Pass Locally But Fail in CI

**Common causes:**
- Environment variables not set
- Different .NET version
- Missing dependencies

### Test Coverage Reports Not Generated

**Solution:**
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

---

*Comprehensive testing ensures reliable AI agents! 🧪✅*
