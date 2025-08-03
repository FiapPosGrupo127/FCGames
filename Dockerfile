FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos dos projetos individuais
COPY src/FCGames.API/*.csproj ./FCGames.API/
COPY src/FCGames.Application/*.csproj ./FCGames.Application/
COPY src/FCGames.Domain/*.csproj ./FCGames.Domain/
COPY src/FCGames.Infrastructure/*.csproj ./FCGames.Infrastructure/

# Restaura os pacotes
RUN dotnet restore ./FCGames.API/FCGames.API.csproj

# Copia o restante do código
COPY src/ ./src/
WORKDIR /app/src/FCGames.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8010
ENTRYPOINT ["dotnet", "FCGames.API.dll"]