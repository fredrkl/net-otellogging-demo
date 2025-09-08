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
dotnet add package OpenTelemetry
dotnet add package OpenTelemetry.Exporter.Console
```

Now if we want to send the logs to an OTel collector, we can use the:

```bash
dotnet add package OpenTelemetry.Exporter.OpenTelemetryProtocol
```

### OTel logging setup

In order to test the OTel setup we will use the OpenTelemetry Collector, which
can be run as a Docker container. The collector can receive logs, metrics, and
traces.

```bash
docker run --rm -p 4317:4317 -p 4318:4318 \
  -v "$(pwd)/otel-collector-config.yaml":/etc/otelcol/config.yaml \
  otel/opentelemetry-collector:latest
```

If your app is running on your local machine and the collector is running in
Docker, you can still use `localhost` as the endpoint:

```sh
export OTEL_EXPORTER_OTLP_ENDPOINT="http://localhost:4317"
```
