FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Account_Removal_Service/Account_Removal_Service.csproj", "Account_Removal_Service/"]

RUN dotnet restore "Account_Removal_Service/Account_Removal_Service.csproj"
COPY . .
WORKDIR "/src/Account_Removal_Service"
RUN dotnet build "Account_Removal_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Account_Removal_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Account_Removal_Service.dll"]