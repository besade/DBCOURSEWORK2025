FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY src/*.sln . 
COPY src/Shop/Shop.csproj ./Shop/

RUN dotnet restore Shop.sln

COPY src/Shop/ ./Shop/

WORKDIR /app/Shop 
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Shop.dll"]