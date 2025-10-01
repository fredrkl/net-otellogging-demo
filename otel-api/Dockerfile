FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:123b43e4d9775451d8ed63af324a42132707b4edb14770bcdf5c85cf55bc45f1 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

## Copy the remaining source code and build
COPY . ./
RUN dotnet publish -c Release -o out

## Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:366204b1d249aa2615f4942c8549677a1f3e6231829f274aa8829fc048f38d8c AS runtime
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "otel-api.dll"]
