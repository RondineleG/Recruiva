# ============================================================
# Stage 1: Build
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar arquivos de projeto primeiro (cache de camadas Docker)
COPY ["src/Recruiva.Web/Recruiva.Web.csproj", "src/Recruiva.Web/"]
COPY ["src/Recruiva.Core/Recruiva.Core.csproj", "src/Recruiva.Core/"]
COPY ["src/Recruiva.Web.Client/Recruiva.Web.Client.csproj", "src/Recruiva.Web.Client/"]

# Restaurar pacotes
RUN dotnet restore "src/Recruiva.Web/Recruiva.Web.csproj"

# Copiar código fonte
COPY ["src/Recruiva.Core/", "src/Recruiva.Core/"]
COPY ["src/Recruiva.Web.Client/", "src/Recruiva.Web.Client/"]
COPY ["src/Recruiva.Web/", "src/Recruiva.Web/"]

# Build em modo Release
WORKDIR "/src/src/Recruiva.Web"
RUN dotnet build "Recruiva.Web.csproj" -c Release -o /app/build --no-restore

# Publish
RUN dotnet publish "Recruiva.Web.csproj" -c Release -o /app/publish --no-restore \
    /p:UseAppHost=false

# ============================================================
# Stage 2: Runtime
# ============================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Criar diretório para uploads
RUN mkdir -p /app/wwwroot/uploads

# Copiar published output do stage de build
COPY --from=build /app/publish .

# Expor portas
EXPOSE 80
EXPOSE 443

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=60s --retries=3 \
  CMD curl --fail http://localhost:80/ || exit 1

# Variáveis de ambiente padrão
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Entry point
ENTRYPOINT ["dotnet", "Recruiva.Web.dll"]
