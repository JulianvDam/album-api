# Base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy and restore project files
COPY . .
RUN dotnet restore "Album.Api/Album.Api.csproj"

# Publish and build the project
RUN dotnet publish "Album.Api/Album.Api.csproj" -c Release -o /app/build

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Album.Api.dll"]