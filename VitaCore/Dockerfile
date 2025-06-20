# ?? Runtime base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# ??? Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiem fi?ierele .csproj ?i appsettings
COPY VitaCore.csproj ./
COPY appsettings.json ./

# Restore dependencies
RUN dotnet restore VitaCore.csproj

# Copiem tot restul fi?ierelor
COPY . .

# Build proiectul
RUN dotnet build VitaCore.csproj -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish VitaCore.csproj -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .        # Copie tot publish
COPY appsettings.json ./                  # Asta îl asigur? în folderul final

ENTRYPOINT ["dotnet", "VitaCore.dll"]
