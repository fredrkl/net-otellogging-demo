FROM mcr.microsoft.com/dotnet/sdk:10.0@sha256:d599d654aa6fd7baabe54b4272dc475768307ef4d8a07d6e9f9b50b7b1758059 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

## Copy the remaining source code and build
COPY . ./
RUN dotnet publish -c Release -o out

## Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0@sha256:5b558c0d5f65031e34528f0c68c954f03f4db77ee18504cad183d239328f55ca AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "otel-api.dll"]
