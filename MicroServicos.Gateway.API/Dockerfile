FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MicroServicos.Gateway.API/MicroServicos.Gateway.API.csproj", "MicroServicos.Gateway.API/"]
RUN dotnet restore "MicroServicos.Gateway.API/MicroServicos.Gateway.API.csproj"
COPY . .
WORKDIR "/src/MicroServicos.Gateway.API"
RUN dotnet build "MicroServicos.Gateway.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroServicos.Gateway.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroServicos.Gateway.API.dll"]