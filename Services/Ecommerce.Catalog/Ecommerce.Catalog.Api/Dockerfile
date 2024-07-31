# Dockerfile for Catalog Api
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Services/Ecommerce.Catalog/Ecommerce.Catalog.Api/Ecommerce.Catalog.Api.csproj", "Services/Ecommerce.Catalog/Ecommerce.Catalog.Api/"]
COPY ["Services/Ecommerce.Catalog/Ecommerce.Catalog.Application/Ecommerce.Catalog.Application.csproj", "Services/Ecommerce.Catalog/Ecommerce.Catalog.Application/"]
COPY ["Services/Ecommerce.Catalog/Ecommerce.Catalog.Domain/Ecommerce.Catalog.Domain.csproj", "Services/Ecommerce.Catalog/Ecommerce.Catalog.Domain/"]
COPY ["Services/Ecommerce.Catalog/Ecommerce.Catalog.Infrastructure/Ecommerce.Catalog.Infrastructure.csproj", "Services/Ecommerce.Catalog/Ecommerce.Catalog.Infrastructure/"]
COPY ["Infrastructure/Ecommerce.Common/Ecommerce.Common.csproj", "Infrastructure/Ecommerce.Common/"]


RUN dotnet restore "Services/Ecommerce.Catalog/Ecommerce.Catalog.Api/Ecommerce.Catalog.Api.csproj"
COPY . .
WORKDIR "/src/Services/Ecommerce.Catalog/Ecommerce.Catalog.Api"
RUN dotnet build "Ecommerce.Catalog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.Catalog.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Catalog.Api.dll"]
