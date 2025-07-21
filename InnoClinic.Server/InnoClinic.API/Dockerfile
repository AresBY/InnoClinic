# Runtime-образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build-образ
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj-файлы
COPY InnoClinic.Server/InnoClinic.API/InnoClinic.API.csproj InnoClinic.Server/InnoClinic.API/
COPY InnoClinic.Server/InnoClinic.Application/InnoClinic.Application.csproj InnoClinic.Server/InnoClinic.Application/
COPY InnoClinic.Server/InnoClinic.Infrastructure/InnoClinic.Infrastructure.csproj InnoClinic.Server/InnoClinic.Infrastructure/
COPY InnoClinic.Server/InnoClinic.Domain/InnoClinic.Domain.csproj InnoClinic.Server/InnoClinic.Domain/

# Восстанавливаем зависимости
RUN dotnet restore "InnoClinic.Server/InnoClinic.API/InnoClinic.API.csproj"

# Копируем все остальные файлы
COPY . .

# Сборка
WORKDIR /src/InnoClinic.Server/InnoClinic.API
RUN dotnet build "InnoClinic.API.csproj" -c Release -o /app/build

# Публикация
RUN dotnet publish "InnoClinic.API.csproj" -c Release -o /app/publish

# Финальный runtime-образ
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InnoClinic.API.dll"]
