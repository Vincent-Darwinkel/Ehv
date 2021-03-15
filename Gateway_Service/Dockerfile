FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Gateway_Service/Gateway_Service.csproj", "Gateway_Service/"]

RUN dotnet restore "Gateway_Service/Gateway_Service.csproj"
COPY . .
WORKDIR "/src/Gateway_Service"
RUN dotnet build "Gateway_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateway_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway_Service.dll"]