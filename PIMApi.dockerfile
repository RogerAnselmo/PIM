FROM mcr.microsoft.com/dotnet/core/sdk:5 AS build

COPY ./src /src

WORKDIR /src/PIM.Api

RUN dotnet restore
RUN dotnet publish -c Release -o publish

FROM mcr.microsoft.com/dotnet/core/sdk:5
COPY --from=build /src/PIM.Api/publish /app

WORKDIR /app

ENV ASPNETCORE_URLS="http://*:5000"
EXPOSE 5000

ENTRYPOINT [ "dotnet", "/app/PIM.Api.dll" ]