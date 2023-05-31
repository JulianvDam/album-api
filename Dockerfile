# Base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Album.Api/Album.Api.csproj"
RUN dotnet build "Album.Api/Album.Api.csproj" -c Release -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish "Album.Api/Album.Api.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Album.Api.dll"]
