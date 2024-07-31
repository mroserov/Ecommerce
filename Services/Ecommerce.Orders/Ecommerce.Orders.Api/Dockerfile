# Dockerfile for Order Api
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Services/Ecommerce.Orders/Ecommerce.Orders.Api/Ecommerce.Orders.Api.csproj", "Services/Ecommerce.Orders/Ecommerce.Orders.Api/"]
COPY ["Services/Ecommerce.Orders/Ecommerce.Orders.Application/Ecommerce.Orders.Application.csproj", "Services/Ecommerce.Orders/Ecommerce.Orders.Application/"]
COPY ["Services/Ecommerce.Orders/Ecommerce.Orders.Domain/Ecommerce.Orders.Domain.csproj", "Services/Ecommerce.Orders/Ecommerce.Orders.Domain/"]
COPY ["Services/Ecommerce.Orders/Ecommerce.Orders.Infrastructure/Ecommerce.Orders.Infrastructure.csproj", "Services/Ecommerce.Orders/Ecommerce.Orders.Infrastructure/"]
COPY ["Infrastructure/Ecommerce.Common/Ecommerce.Common.csproj", "Infrastructure/Ecommerce.Common/"]


RUN dotnet restore "Services/Ecommerce.Orders/Ecommerce.Orders.Api/Ecommerce.Orders.Api.csproj"
COPY . .
WORKDIR "/src/Services/Ecommerce.Orders/Ecommerce.Orders.Api"
RUN dotnet build "Ecommerce.Orders.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.Orders.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Orders.Api.dll"]
