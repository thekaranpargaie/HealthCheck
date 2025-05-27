# HealthCheck.Api/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HealthCheck.Api/HealthCheck.Api.csproj", "HealthCheck.Api/"]
COPY ["HealthCheck.Main/HealthCheck.Main.csproj", "HealthCheck.Main/"]
RUN dotnet restore "HealthCheck.Api/HealthCheck.Api.csproj"
COPY . .
WORKDIR "/src/HealthCheck.Api"
RUN dotnet publish "HealthCheck.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HealthCheck.Api.dll"]