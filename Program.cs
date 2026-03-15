using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddOptionsWithValidateOnStart<OpenAiSettings>()
    .Bind(builder.Configuration.GetSection("Providers:OpenAI"))
    .ValidateDataAnnotations();

builder.Services.AddScoped<OpenAIGateway>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", (OpenAIGateway gateway) =>
{
    return Results.Ok(gateway.ExecutePrompt("Alo mundo"));
});

app.Run();

internal class OpenAIGateway(IOptions<OpenAiSettings> openAiSettings)
{
    private readonly string _baseAddress = openAiSettings.Value.BaseAddress;
    private readonly string _apiKey = openAiSettings.Value.ApiKey;

    public Task<string> ExecutePrompt(string prompt)
    {
        return Task.FromResult("Resultado "+ _baseAddress);
    }
}

public class OpenAiSettings
{
    [Required]
    public string ApiKey { get; set; }

    [Required]
    public string BaseAddress { get; set; }
}






