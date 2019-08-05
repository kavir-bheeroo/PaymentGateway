FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Simulator/BankSimulator/BankSimulator.csproj", "src/Simulator/BankSimulator/"]
RUN dotnet restore "src/Simulator/BankSimulator/BankSimulator.csproj"
COPY . .
WORKDIR "/src/src/Simulator/BankSimulator"
RUN dotnet build "BankSimulator.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BankSimulator.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BankSimulator.dll"]