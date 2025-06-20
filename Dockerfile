# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy and restore .csproj
COPY ["VitaCore/VitaCore.csproj", "VitaCore/"]
RUN dotnet restore "VitaCore/VitaCore.csproj"

# Copy all source files
COPY . .
WORKDIR "/src/VitaCore"
RUN dotnet build "VitaCore.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "VitaCore.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VitaCore.dll"]
