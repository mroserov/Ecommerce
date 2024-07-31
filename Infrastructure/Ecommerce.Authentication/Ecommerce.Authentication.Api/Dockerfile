# Etapa base: configuración de la imagen base para la ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build: configuración de la imagen para la construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY ["Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Api/Ecommerce.Authentication.Api.csproj", "Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Api/"]
COPY ["Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Application/Ecommerce.Authentication.Application.csproj", "Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Application/"]
COPY ["Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Domain/Ecommerce.Authentication.Domain.csproj", "Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Domain/"]
COPY ["Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Infrastructure/Ecommerce.Authentication.Infrastructure.csproj", "Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Infrastructure/"]
COPY ["Infrastructure/Ecommerce.Common/Ecommerce.Common.csproj", "Infrastructure/Ecommerce.Common/"]

RUN dotnet restore "Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Api/Ecommerce.Authentication.Api.csproj"

# Copiar el resto del código fuente y compilar el proyecto
COPY . .
WORKDIR "/src/Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Api"
RUN dotnet build "Ecommerce.Authentication.Api.csproj" -c Release -o /app/build

# Etapa de publicación: publicar la aplicación en el entorno de producción
FROM build AS publish
RUN dotnet publish "Ecommerce.Authentication.Api.csproj" -c Release -o /app/publish

# Etapa final: configurar la imagen final para la ejecución de la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Authentication.Api.dll"]
