# Fase de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos de proyecto y restaurar
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código y publicar en modo Release
COPY . ./
RUN dotnet publish -c Release -o out

# Fase de ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# --- AJUSTES PARA RAILWAY ---
# Forzamos el entorno a Production
ENV ASPNETCORE_ENVIRONMENT=Production

# Railway inyecta la variable PORT. No es necesario definir un default aquí,
# es mejor dejar que ASP.NET Core escuche en el puerto que Railway le asigne.
ENV ASPNETCORE_URLS=http://+:8080

# Exponemos el puerto estándar, pero Railway lo sobrescribirá internamente.
EXPOSE 8080

ENTRYPOINT ["dotnet", "InvestigacionClinica.dll"]