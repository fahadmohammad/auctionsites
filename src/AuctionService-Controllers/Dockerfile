FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY Carsties.sln ./
COPY src/AuctionService-Controllers/AuctionService-Controllers.csproj ./src/AuctionService-Controllers/
COPY src/Contracts/Contracts.csproj ./src/Contracts/

RUN dotnet restore ./src/AuctionService-Controllers/AuctionService-Controllers.csproj

COPY . .
RUN dotnet publish ./src/AuctionService-Controllers/AuctionService-Controllers.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AuctionService-Controllers.dll"]