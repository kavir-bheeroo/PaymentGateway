FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Gateway/Gateway.Host/Gateway.Host.csproj", "src/Gateway/Gateway.Host/"]
COPY ["src/Gateway/Gateway.Data.Dapper/Gateway.Data.Dapper.csproj", "src/Gateway/Gateway.Data.Dapper/"]
COPY ["src/Common/Gateway.Common/Gateway.Common.csproj", "src/Common/Gateway.Common/"]
COPY ["src/Gateway/Gateway.Data.Contracts/Gateway.Data.Contracts.csproj", "src/Gateway/Gateway.Data.Contracts/"]
COPY ["src/Gateway/Gateway.Contracts.Public/Gateway.Contracts.Public.csproj", "src/Gateway/Gateway.Contracts.Public/"]
COPY ["src/Gateway/Gateway.Contracts/Gateway.Contracts.csproj", "src/Gateway/Gateway.Contracts/"]
COPY ["src/Gateway/Gateway.Data.Migrations/Gateway.Data.Migrations.csproj", "src/Gateway/Gateway.Data.Migrations/"]
COPY ["src/Gateway/Gateway.Core/Gateway.Core.csproj", "src/Gateway/Gateway.Core/"]
RUN dotnet restore "src/Gateway/Gateway.Host/Gateway.Host.csproj"
COPY . .
WORKDIR "/src/src/Gateway/Gateway.Host"
RUN dotnet build "Gateway.Host.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Gateway.Host.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Gateway.Host.dll"]