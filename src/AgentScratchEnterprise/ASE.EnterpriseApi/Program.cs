using System.Net;
using ASE.EnterpriseApi.Routes;
using ASE.Libraries;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(settings => settings.AddServerHeader = false);
builder.Services.AddTransient<ILogger>(p =>
{
    var loggerFactory = p.GetRequiredService<ILoggerFactory>();
    return loggerFactory.CreateLogger("AgentEnterpriseApi");
});
builder.Services.Configure<ForwardedHeadersOptions>(options =>
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);
builder.Services.AddMemoryCache();
builder.Services.AddHealthChecks();

if (Environment.GetEnvironmentVariable("SEARCH_ENVIRONMENT")?.ToUpper() == "LOCAL")
    builder.Services.AddScoped<ISearchService, DocumentSearchAdapter>();
else
    builder.Services.AddScoped<ISearchService, AzureSearchDocumentSearchAdapter>();

var app = builder.Build();
app.UseForwardedHeaders();
app.MapGroup(RouteNames.BasicRoute)
    .MapBasicEnterpriseApi();
    
app.UseExceptionHandler(options =>
{
    options.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";
        var exception = context.Features.Get<IExceptionHandlerFeature>();
        if (exception != null)
        {
            var message = $"{exception.Error.Message}";
            await context.Response.WriteAsync(message).ConfigureAwait(false);
        }
    });
});
app.MapHealthChecks("/health");
app.Run();
public partial class Program { }
