﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5023

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Heroes/Heroes.API/Heroes.API.csproj", "Services/Heroes/Heroes.API/"]
RUN dotnet restore "Services/Heroes/Heroes.API/Heroes.API.csproj"
COPY . .
WORKDIR "/src/Services/Heroes/Heroes.API"
RUN dotnet build "Heroes.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Heroes.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Heroes.API.dll"]
