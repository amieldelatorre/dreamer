#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Dreamer.Api/Dreamer.Api.csproj", "Dreamer.Api/"]
COPY ["Dreamer.Domain/Dreamer.Domain.csproj", "Dreamer.Domain/"]
COPY ["Dreamer.DataAccess/Dreamer.DataAccess.csproj", "Dreamer.DataAccess/"]
RUN dotnet restore "./Dreamer.Api/Dreamer.Api.csproj"
COPY . .
WORKDIR "/src/Dreamer.Api"
RUN dotnet build "./Dreamer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Dreamer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dreamer.Api.dll"]