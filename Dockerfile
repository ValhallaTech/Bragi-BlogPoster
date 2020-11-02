#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-focal AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Postgres_Demo.csproj", ""]
RUN dotnet restore "./Postgres_Demo.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Postgres_Demo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Postgres_Demo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet Postgres_Demo.dll