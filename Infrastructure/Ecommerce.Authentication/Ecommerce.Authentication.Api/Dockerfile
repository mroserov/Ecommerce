# Etapa base: configuraci�n de la imagen base para la ejecuci�n
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build: configuraci�n de la imagen para la construcci�n
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY ["Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Api/Ecommerce.Authentication.Api.csproj", "Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Api/"]
COPY ["Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Application/Ecommerce.Authentication.Application.csproj", "Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Application/"]
COPY ["Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Domain/Ecommerce.Authentication.Domain.csproj", "Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Domain/"]
COPY ["Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Infrastructure/Ecommerce.Authentication.Infrastructure.csproj", "Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Infrastructure/"]
COPY ["Infrastructure/Ecommerce.Common/Ecommerce.Common.csproj", "Infrastructure/Ecommerce.Common/"]

RUN dotnet restore "Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Api/Ecommerce.Authentication.Api.csproj"

# Copiar el resto del c�digo fuente y compilar el proyecto
COPY . .
WORKDIR "/src/Infrastructure/Ecommerce.Authentication/Ecommerce.Authentication.Api"
RUN dotnet build "Ecommerce.Authentication.Api.csproj" -c Release -o /app/build

# Etapa de publicaci�n: publicar la aplicaci�n en el entorno de producci�n
FROM build AS publish
RUN dotnet publish "Ecommerce.Authentication.Api.csproj" -c Release -o /app/publish

# Etapa final: configurar la imagen final para la ejecuci�n de la aplicaci�n
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Authentication.Api.dll"]