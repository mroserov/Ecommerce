# Etapa base: configuración de la imagen base para la ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Etapa de build: configuración de la imagen para la construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY ["Ecommerce.Authentication.Api/Ecommerce.Authentication.Api.csproj", "Ecommerce.Authentication.Api/"]
COPY ["Ecommerce.Authentication.Application/Ecommerce.Authentication.Application.csproj", "Ecommerce.Authentication.Application/"]
COPY ["Ecommerce.Authentication.Domain/Ecommerce.Authentication.Domain.csproj", "Ecommerce.Authentication.Domain/"]
COPY ["Ecommerce.Authentication.Infrastructure/Ecommerce.Authentication.Infrastructure.csproj", "Ecommerce.Authentication.Infrastructure/"]
RUN dotnet restore "Ecommerce.Authentication.Api/Ecommerce.Authentication.Api.csproj"

# Copiar el resto del código fuente y compilar el proyecto
COPY . .
WORKDIR "/src/Ecommerce.Authentication.Api"
RUN dotnet build "Ecommerce.Authentication.Api.csproj" -c Release -o /app/build

# Etapa de publicación: publicar la aplicación en el entorno de producción
FROM build AS publish
RUN dotnet publish "Ecommerce.Authentication.Api.csproj" -c Release -o /app/publish

# Etapa final: configurar la imagen final para la ejecución de la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Authentication.Api.dll"]
