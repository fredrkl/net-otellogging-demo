using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Environment
IWebHostEnvironment environment = builder.Environment;
environment.ApplicationName = "Otel API";

// IHost
ConfigureHostBuilder host = builder.Host;
ConfigureWebHostBuilder webHost = builder.WebHost;

// The ConfigurationManager is used to manage application configuration,
// read and register configuration sources.
ConfigurationManager configurationManager = builder.Configuration;

// We can also create a configutation Builder from scratch and add configuration sources to it, and the add it to the WebApplicationBuilder configuration.
ConfigurationBuilder customConfigurationBuilder = new();
customConfigurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
{
    { "AnotherCustomSetting", "AnotherCustomValue" }
});

// IConfigurationRoot extends IConfiguration and represents a configuration that can be reloaded.
IConfigurationRoot configurationRoot = customConfigurationBuilder.Build();
configurationManager.AddConfiguration(configurationRoot);

var otlpEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT");

// Logging
builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeScopes = true;
    options.ParseStateValues = true;
    options.IncludeFormattedMessage = true;
    options.AddConsoleExporter();
    options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: "OTEL Demo Application", serviceVersion: "1.0.0"));

    if (!string.IsNullOrEmpty(otlpEndpoint))
    {
        Console.WriteLine($"Using OTLP endpoint: {otlpEndpoint}");
        options.AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Endpoint = new Uri(otlpEndpoint);
            otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        });
    }
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

Console.WriteLine(app.Configuration["MyCustomSetting"]);
Console.WriteLine(app.Configuration["AnotherCustomSetting"]);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (ILogger<WeatherForecast> logger) =>
{
    logger.LogError("A critical error occured");
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
