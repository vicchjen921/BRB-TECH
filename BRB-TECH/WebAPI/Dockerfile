#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BRB-TECH/WebAPI/WebAPI.csproj", "BRB-TECH/WebAPI/"]
COPY ["BRB-TECH/Application/Application.csproj", "BRB-TECH/Application/"]
COPY ["BRB-TECH/Domain/Domain.csproj", "BRB-TECH/Domain/"]
COPY ["BRB-TECH/Infrastructure/Infrastructure.csproj", "BRB-TECH/Infrastructure/"]
RUN dotnet restore "./BRB-TECH/WebAPI/WebAPI.csproj"
COPY . .
WORKDIR "/src/BRB-TECH/WebAPI"
RUN dotnet build "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY entrypoint.sh .
RUN chmod +x ./entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]