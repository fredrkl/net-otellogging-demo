# Logging demo in .NET

This repo demonstrates how logging can be extended to support the OpenTelemetry
standard in .NET applications.

## Hosting

When creating a WebApplicationBuilder, we get access to the
`ConfigureHostBuilder` through the `builder.Host` property, and the
`ConfigureWebHostBuilder` through the `builder.WebHost` property. The main
differeces are:

- `ConfigureWebHostBuilder` is an extension method that allows us to configure
  the web hosting part of `IHostBuilder`.
- `ConfigureHostBuilder` is an extension method that allows us to configure the
  generic host infrastructure part of `IHostBuilder`.
