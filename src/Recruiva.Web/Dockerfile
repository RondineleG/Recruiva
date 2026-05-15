# ============================================================
# Stage 1: Build
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar arquivos de projeto primeiro (cache de camadas Docker)
COPY ["src/Recruiva.Web/Recruiva.Web.csproj", "Recruiva.Web/"]
COPY ["src/Recruiva.Core/Recruiva.Core.csproj", "Recruiva.Core/"]
COPY ["src/Recruiva.Web.Client/Recruiva.Web.Client.csproj", "Recruiva.Web.Client/"]

# Restaurar pacotes
RUN dotnet restore "Recruiva.Web/Recruiva.Web.csproj"

# Limpar cache do NuGet para evitar problemas de fallback folders
RUN dotnet nuget locals all --clear

# Copiar código fonte
COPY ["src/Recruiva.Core/", "Recruiva.Core/"]
COPY ["src/Recruiva.Web.Client/", "Recruiva.Web.Client/"]
COPY ["src/Recruiva.Web/", "Recruiva.Web/"]

# Publish diretamente (inclui build)
WORKDIR "/src/Recruiva.Web"
RUN mkdir -p bin/Release/net9.0/pt-br /app/publish/pt-br
RUN dotnet publish "Recruiva.Web.csproj" -c Release -o /app/publish \
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

# Variáveis de ambiente padrão
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Entry point
ENTRYPOINT ["dotnet", "Recruiva.Web.dll"]
