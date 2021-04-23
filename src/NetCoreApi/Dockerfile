#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NetCoreApi/NetCoreApi.csproj", "NetCoreApi/"]
RUN dotnet restore "NetCoreApi/NetCoreApi.csproj"
COPY . .
WORKDIR "/src/NetCoreApi"
RUN dotnet build "NetCoreApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetCoreApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCoreApi.dll"]