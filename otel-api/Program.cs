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

// The ConfigurationManager was introduced in .NET 6 and implements both IConfiguration and IConfigurationBuilder.
// We can use it to read configuration values and add new configuration sources.
// Since we used the WebApplication.CreateBuilder method to create the builder,
// it already has some configuration sources added by default. The sources added by default are:
// 1. appsettings.json
// 2. appsettings.{Environment}.json
// 3. User secrets (if the environment is Development and the project has user secrets enabled)
// 4. Environment variables
// 5. Command-line arguments

// We can also create a configutation Builder from scratch and add configuration sources to it, and the add it to the WebApplicationBuilder configuration.
ConfigurationBuilder customConfigurationBuilder = new();
customConfigurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
{
    { "AnotherCustomSetting", "AnotherCustomValue" }
});

// IConfigurationRoot extends IConfiguration and represents a configuration that can be reloaded.
IConfigurationRoot configurationRoot = customConfigurationBuilder.Build();
configurationManager.AddConfiguration(configurationRoot);

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

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
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
