# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Render asigna el puerto en la variable de entorno PORT
ENV ASPNETCORE_URLS=http://+:${PORT}

COPY --from=build /app/out .

# Exponer puerto 8080 (documentativo, Render ignora este valor)
EXPOSE 8080

ENTRYPOINT ["dotnet", "CotizadorEquipoContratista.dll"]
