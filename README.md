# Logging demo in .NET

This repo demonstrates how logging can be extended to support the OpenTelemetry
standard in .NET applications.

## Hosting

### Host builders

When creating a WebApplicationBuilder, we get access to the
`ConfigureHostBuilder` through the `builder.Host` property, and the
`ConfigureWebHostBuilder` through the `builder.WebHost` property. The main
differeces are:

- `ConfigureWebHostBuilder` is an extension method that allows us to configure
  the web hosting part of `IHostBuilder`.
- `ConfigureHostBuilder` is an extension method that allows us to configure the
  generic host infrastructure part of `IHostBuilder`.

### Configuration Manager

Additionally, we access the `ConfigurationManager` through the
`builder.Configuration` property, which allows us to read configuration
settings from various sources and add new sources.

### Logging

The `builder.Logging` property provides access to the logging configuration,
allowing us to set up logging providers, configure log levels, and customize
logging behavior.

In this demo we want to add OptenTelemetry(OTel) logging support, and send the
logs to an OTel collector. When looking at the OpenTelemetry documentation, we
see two different ways of adding OTel support.

**Builder.Services**  

- Used for adding OpenTelemetry **tracing** and **metrics**.
- This registers background services for collecting and exporting
  traces/metrics.

```cs
builder.Services.AddOpenTelemetry()
    .WithTracing(...)
    .WithMetrics(...);
```

**Builder.Logging**  

- Used for adding OpenTelemetry as a **logging provider**.
- Example:  

```cs
builder.Logging.AddOpenTelemetry(...);
```

- This sends your application logs through OpenTelemetry.

We start by adding the necessary NuGet packages:

```bash
dotnet add package OpenTelemetry.Extensions.Hosting
dotnet add package OpenTelemetry.Logs
dotnet add package OpenTelemetry.Exporter.Console
```
