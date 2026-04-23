using System.ComponentModel.DataAnnotations;
using ASE.EnterpriseApi.Options;
using Microsoft.Extensions.Configuration;

namespace ASE.EnterpriseApi.Tests;

public class CorsOptionsTests
{
    [Fact]
    public void SectionName_IsCorrect()
    {
        Assert.Equal("Cors", CorsOptions.SectionName);
    }

    [Fact]
    public void AllowedOrigins_Required_FailsValidationWhenNull()
    {
        var options = new CorsOptions { AllowedOrigins = null! };
        var results = ValidateOptions(options);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(CorsOptions.AllowedOrigins)));
    }

    [Fact]
    public void AllowedOrigins_Required_FailsValidationWhenEmpty()
    {
        var options = new CorsOptions { AllowedOrigins = [] };
        var results = ValidateOptions(options);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(CorsOptions.AllowedOrigins)));
    }

    [Fact]
    public void AllowedOrigins_PassesValidationWithAtLeastOneOrigin()
    {
        var options = new CorsOptions { AllowedOrigins = ["http://localhost:5173"] };
        var results = ValidateOptions(options);
        Assert.Empty(results);
    }

    [Fact]
    public void AllowedOrigins_PassesValidationWithMultipleOrigins()
    {
        var options = new CorsOptions
        {
            AllowedOrigins =
            [
                "http://localhost:5173",
                "https://localhost:5173",
                "http://localhost:3000"
            ]
        };
        var results = ValidateOptions(options);
        Assert.Empty(results);
    }

    [Fact]
    public void BindFromConfiguration_PopulatesAllowedOrigins()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Cors:AllowedOrigins:0"] = "http://localhost:5173",
                ["Cors:AllowedOrigins:1"] = "https://localhost:5173",
            })
            .Build();

        var options = config.GetSection(CorsOptions.SectionName).Get<CorsOptions>();

        Assert.NotNull(options);
        Assert.Equal(2, options.AllowedOrigins.Length);
        Assert.Contains("http://localhost:5173", options.AllowedOrigins);
        Assert.Contains("https://localhost:5173", options.AllowedOrigins);
    }

    private static List<ValidationResult> ValidateOptions(CorsOptions options)
    {
        var results = new List<ValidationResult>();
        var ctx = new ValidationContext(options);
        Validator.TryValidateObject(options, ctx, results, validateAllProperties: true);
        return results;
    }
}
