FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:bb42ae2c058609d1746baf24fe6864ecab0686711dfca1f4b7a99e367ab17162 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

## Copy the remaining source code and build
COPY . ./
RUN dotnet publish -c Release -o out

## Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:1af4114db9ba87542a3f23dbb5cd9072cad7fcc8505f6e9131d1feb580286a6f AS runtime
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 8000
ENTRYPOINT ["dotnet", "otel-api.dll"]
