# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY GatewayAPI /src/GatewayAPI
COPY NuGet.config .
COPY LocalNugetPackages /src/LocalNugetPackages

WORKDIR /src/GatewayAPI

# Restore, build, publish
RUN dotnet restore "GatewayAPI.csproj"
RUN dotnet publish "GatewayAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80

# Copy published app
COPY --from=build /app/publish .

# Entry point
ENTRYPOINT ["dotnet", "GatewayAPI.dll"]
