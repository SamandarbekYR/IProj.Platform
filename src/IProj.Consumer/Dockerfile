FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY ./src ./

WORKDIR /src/IProj.Consumer

RUN dotnet restore

RUN dotnet publish -c Release -o output

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS serve
WORKDIR /app
COPY --from=build /src/IProj.Consumer/output .

EXPOSE 8080
EXPOSE 443

ENTRYPOINT [ "dotnet", "IProj.Consumer.dll" ]