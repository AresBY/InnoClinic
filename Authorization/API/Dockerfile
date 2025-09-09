# -------------------- Build stage --------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем исходники микросервиса
COPY Authorization /src/Authorization
COPY ["NuGet.config", "NuGet.config"]
COPY LocalNugetPackages /src/LocalNugetPackages

WORKDIR /src/Authorization/API

# Восстанавливаем зависимости и билдим проект
RUN dotnet restore "API.csproj"
RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# -------------------- Runtime stage --------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Экспонируем порт внутри контейнера (80 для ASP.NET Core)
EXPOSE 80

# Копируем опубликованные файлы из build stage
COPY --from=build /app/publish .

# Точка входа
ENTRYPOINT ["dotnet", "API.dll"]
