# Dockerfile

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Ecommerce.Basket/Ecommerce.Basket.Api/Ecommerce.Basket.Api.csproj", "Services/Ecommerce.Basket/Ecommerce.Basket.Api/"]
COPY ["Services/Ecommerce.Basket/Ecommerce.Basket.Application/Ecommerce.Basket.Application.csproj", "Services/Ecommerce.Basket/Ecommerce.Basket.Application/"]
COPY ["Services/Ecommerce.Basket/Ecommerce.Basket.Domain/Ecommerce.Basket.Domain.csproj", "Services/Ecommerce.Basket/Ecommerce.Basket.Domain/"]
COPY ["Services/Ecommerce.Basket/Ecommerce.Basket.Infrastructure/Ecommerce.Basket.Infrastructure.csproj", "Services/Ecommerce.Basket/Ecommerce.Basket.Infrastructure/"]

RUN dotnet restore "Services/Ecommerce.Basket/Ecommerce.Basket.Api/Ecommerce.Basket.Api.csproj"
COPY . .
WORKDIR "/src/Services/Ecommerce.Basket/Ecommerce.Basket.Api"
RUN dotnet build "Ecommerce.Basket.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.Basket.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Basket.Api.dll"]
