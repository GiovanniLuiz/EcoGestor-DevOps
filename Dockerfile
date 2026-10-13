FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY src/EcoGestorApi/EcoGestorApi.csproj src/EcoGestorApi/
RUN dotnet restore src/EcoGestorApi/EcoGestorApi.csproj
COPY src/ src/
RUN dotnet publish src/EcoGestorApi/EcoGestorApi.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
USER root
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
USER app
HEALTHCHECK --interval=10s --timeout=5s --start-period=60s --retries=12 CMD curl --fail --silent http://localhost:8080/health/ready || exit 1
ENTRYPOINT ["dotnet", "EcoGestorApi.dll"]
