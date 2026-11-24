
# Этап 1: сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["TaskFlow.Api.csproj", "./"]

RUN dotnet restore "TaskFlow.Api.csproj"

COPY . .

RUN dotnet publish "TaskFlow.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# Этап 2: итоговый образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

# Каталог для постоянного хранения SQLite
RUN mkdir -p /data && chown $APP_UID:$APP_UID /data

ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

USER $APP_UID

ENTRYPOINT ["dotnet", "TaskFlow.Api.dll"]
