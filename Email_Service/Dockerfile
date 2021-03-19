FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Email_Service/Email_Service.csproj", "Email_Service/"]

RUN dotnet restore "Email_Service/Email_Service.csproj"
COPY . .
WORKDIR "/src/Email_Service"
RUN dotnet build "Email_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Email_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Email_Service.dll"]