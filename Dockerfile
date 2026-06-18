# ---- Base runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000

# ---- Build ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["curtains-be/curtains-be.csproj", "curtains-be/"]
RUN dotnet restore "curtains-be/curtains-be.csproj"
COPY . .
WORKDIR "/src/curtains-be"
RUN dotnet build "./curtains-be.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ---- Publish ----
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./curtains-be.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---- Final ----
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Render inject biến $PORT lúc chạy; app phải lắng nghe đúng cổng đó.
ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-10000} dotnet curtains-be.dll"]
