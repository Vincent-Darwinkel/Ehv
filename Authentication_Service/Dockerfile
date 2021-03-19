FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Authentication_Service/Authentication_Service.csproj", "Authentication_Service/"]

RUN dotnet restore "Authentication_Service/Authentication_Service.csproj"
COPY . .
WORKDIR "/src/Authentication_Service"
RUN dotnet build "Authentication_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authentication_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication_Service.dll"]