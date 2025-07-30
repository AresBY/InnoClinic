FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем проекты
COPY Offices ./Offices

# Копируем nuget.config и локальные пакеты
COPY ["NuGet.config", "NuGet.config"]
COPY LocalNugetPackages ./LocalNugetPackages

# Указываем restore с config и локальным источником
WORKDIR /src/Offices/API
RUN dotnet restore "API.csproj" --configfile /src/NuGet.config
RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
