﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5050

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ApiGateways/Web.ApiGateway/Web.ApiGateway.csproj", "ApiGateways/Web.ApiGateway/"]
COPY ["Common/CommonExtensions/CommonExtensions.csproj", "Common/CommonExtensions/CommonExtensions.csproj"]
RUN dotnet restore "ApiGateways/Web.ApiGateway/Web.ApiGateway.csproj"
COPY . .
WORKDIR "/src/ApiGateways/Web.ApiGateway"
RUN dotnet build "Web.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.ApiGateway.dll"]
