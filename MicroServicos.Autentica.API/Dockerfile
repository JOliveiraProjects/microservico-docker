FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MicroServicos.Autentica.API/MicroServicos.Autentica.API.csproj", "MicroServicos.Autentica.API/"]
RUN dotnet restore "MicroServicos.Autentica.API/MicroServicos.Autentica.API.csproj"
COPY . .
WORKDIR "/src/MicroServicos.Autentica.API"
RUN dotnet build "MicroServicos.Autentica.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroServicos.Autentica.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroServicos.Autentica.API.dll"]