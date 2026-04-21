using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Agents.AI;
using Spectre.Console;

#region Environment variables

var endpoint = Environment.GetEnvironmentVariable("ENDPOINT");
ArgumentException.ThrowIfNullOrEmpty(endpoint, "Please set the ENDPOINT environment variable.");
var deploymentName = Environment.GetEnvironmentVariable("DEPLOYMENTNAME");
ArgumentException.ThrowIfNullOrEmpty(deploymentName, "Please set the DEPLOYMENTNAME environment variable.");

#endregion

AIAgent agent = new AIProjectClient(new Uri(endpoint), new DefaultAzureCredential())
    .AsAIAgent(
        model: deploymentName,
        instructions: "You are a friendly assistant. Keep your answers brief.",
        name: "SimpleAgentToStartWith");
var question = AnsiConsole.Ask<string>("Ask your question",
    "What is the 2nd largest city in Poland by population size?");
AnsiConsole.MarkupLine("[green]Question:[/]" + question);
var answer = await agent.RunAsync(question);
AnsiConsole.MarkupLine("[green]Answer:[/]" + answer);