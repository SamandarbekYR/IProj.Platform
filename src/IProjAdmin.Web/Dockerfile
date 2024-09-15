#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/IProjAdmin.Web/IProjAdmin.Web.csproj", "src/IProjAdmin.Web/"]
COPY ["src/IProj.Service/IProj.Service.csproj", "src/IProj.Service/"]
COPY ["src/IProj.DataAccess/IProj.DataAccess.csproj", "src/IProj.DataAccess/"]
COPY ["src/IProj.Domain/IProj.Domain.csproj", "src/IProj.Domain/"]
RUN dotnet restore "./src/IProjAdmin.Web/IProjAdmin.Web.csproj"
COPY . .
WORKDIR "/src/src/IProjAdmin.Web"
RUN dotnet build "./IProjAdmin.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IProjAdmin.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IProjAdmin.Web.dll"]