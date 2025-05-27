# HealthCheck.Ui/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HealthCheck.Ui/HealthCheck.Ui.csproj", "HealthCheck.Ui/"]
COPY ["HealthCheck.Main/HealthCheck.Main.csproj", "HealthCheck.Main/"]
RUN dotnet restore "HealthCheck.Ui/HealthCheck.Ui.csproj"
COPY . .
WORKDIR "/src/HealthCheck.Ui"
RUN dotnet publish "HealthCheck.Ui.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HealthCheck.Ui.dll"]