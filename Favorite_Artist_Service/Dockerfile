FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Favorite_Artist_Service/Favorite_Artist_Service.csproj", "Favorite_Artist_Service/"]

RUN dotnet restore "Favorite_Artist_Service/Favorite_Artist_Service.csproj"
COPY . .
WORKDIR "/src/Favorite_Artist_Service"
RUN dotnet build "Favorite_Artist_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Favorite_Artist_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Favorite_Artist_Service.dll"]