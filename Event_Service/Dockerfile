FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Event_Service/Event_Service.csproj", "Event_Service/"]

RUN dotnet restore "Event_Service/Event_Service.csproj"
COPY . .
WORKDIR "/src/Event_Service"
RUN dotnet build "Event_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Event_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Event_Service.dll"]