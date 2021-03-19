FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["User_Service/User_Service.csproj", "User_Service/"]

RUN dotnet restore "User_Service/User_Service.csproj"
COPY . .
WORKDIR "/src/User_Service"
RUN dotnet build "User_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "User_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User_Service.dll"]