FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY ./src ./ 

WORKDIR /src/IProjAdmin.Web

RUN dotnet restore

RUN dotnet publish -c Release -o /app/output

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS serve
WORKDIR /app

COPY --from=build /app/output .

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "IProjAdmin.Web.dll"]
