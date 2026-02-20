FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY Carsties.sln ./
COPY src/SearchService-Controllers/SearchService-Controllers.csproj ./src/SearchService-Controllers/
COPY src/Contracts/Contracts.csproj ./src/Contracts/

RUN dotnet restore ./src/SearchService-Controllers/SearchService-Controllers.csproj

COPY . .
RUN dotnet publish ./src/SearchService-Controllers/SearchService-Controllers.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SearchService-Controllers.dll"]