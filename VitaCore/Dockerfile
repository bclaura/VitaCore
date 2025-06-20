# Base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["VitaCore.csproj", "./"]
RUN dotnet restore "VitaCore.csproj"

COPY . .                         # ?? Copiaz? tot inclusiv appsettings.json
RUN dotnet build "VitaCore.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "VitaCore.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# ?? Copiaz? manual fi?ierul config dac? nu a ajuns în publish
COPY appsettings.json .        

ENTRYPOINT ["dotnet", "VitaCore.dll"]
