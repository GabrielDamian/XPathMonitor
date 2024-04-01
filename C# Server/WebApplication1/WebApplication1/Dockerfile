
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/bin/Release/net7.0 .

ENV ASPNETCORE_HTTPS_PORTS=7196
ENTRYPOINT ["dotnet", "WebApplication1.dll"]
