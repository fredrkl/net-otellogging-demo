FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

## Copy the remaining source code and build
COPY . ./
RUN dotnet publish -c Release -o out

## Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 8000
ENTRYPOINT ["dotnet", "otel-api.dll"]
