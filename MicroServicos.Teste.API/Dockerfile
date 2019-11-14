FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MicroServicos.Teste.API/MicroServicos.Teste.API.csproj", "MicroServicos.Teste.API/"]
RUN dotnet restore "MicroServicos.Teste.API/MicroServicos.Teste.API.csproj"
COPY . .
WORKDIR "/src/MicroServicos.Teste.API"
RUN dotnet build "MicroServicos.Teste.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroServicos.Teste.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroServicos.Teste.API.dll"]