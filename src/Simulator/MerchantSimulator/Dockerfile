FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Simulator/MerchantSimulator/MerchantSimulator.csproj", "src/Simulator/MerchantSimulator/"]
COPY ["src/Common/Gateway.Common.Web/Gateway.Common.Web.csproj", "src/Common/Gateway.Common.Web/"]
COPY ["src/Common/Gateway.Common/Gateway.Common.csproj", "src/Common/Gateway.Common/"]
COPY ["src/Common/Gateway.Common.Logging.Serilog/Gateway.Common.Logging.Serilog.csproj", "src/Common/Gateway.Common.Logging.Serilog/"]
COPY ["src/Gateway/Gateway.Contracts.Public/Gateway.Contracts.Public.csproj", "src/Gateway/Gateway.Contracts.Public/"]
COPY ["src/Gateway/Gateway.Client/Gateway.Client.csproj", "src/Gateway/Gateway.Client/"]
RUN dotnet restore "src/Simulator/MerchantSimulator/MerchantSimulator.csproj"
COPY . .
WORKDIR "/src/src/Simulator/MerchantSimulator"
RUN dotnet build "MerchantSimulator.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MerchantSimulator.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MerchantSimulator.dll"]