FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

COPY --from=build /app/out ./

COPY certificate.pfx /app
ENV ASPNETCORE_URLS=https://+:5001
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="damian"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="certificate.pfx"

CMD ["dotnet", "WebApplication1.dll"]
