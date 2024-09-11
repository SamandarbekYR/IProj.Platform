FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY ./src ./

WORKDIR /src/IProj.Web

RUN dotnet restore

RUN dotnet publish -c Release -o output

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS serve
WORKDIR /app
COPY --from=build /src/IProj.Web/output .

EXPOSE 8080
EXPOSE 443

ENTRYPOINT [ "dotnet", "IProj.Web.dll" ]