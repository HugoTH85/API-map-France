#Build stage
FROM bitnami/dotnet:latest as Build

WORKDIR /src

COPY . .

RUN dotnet restore "./carte-france-interactive.csproj" --disable-parallel

RUN dotnet publish "./carte-france-interactive.csproj" -c release -o /app --no-restore

#Serve stage
FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine

WORKDIR /app

COPY --from=Build /app ./

EXPOSE 5000

ENTRYPOINT [ "dotnet","carte-france-interactive.dll" ]