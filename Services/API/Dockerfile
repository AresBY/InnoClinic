FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Services /src/Services
COPY ["NuGet.config", "NuGet.config"]
COPY LocalNugetPackages /src/LocalNugetPackages

WORKDIR /src/Services/API

RUN dotnet restore "API.csproj"
RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
