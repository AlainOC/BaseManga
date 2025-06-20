# Etapa base con runtime de .NET 9 preview
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app

# Etapa de build con SDK de .NET 9 preview
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish --no-restore

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MiMangaBot.dll"] 