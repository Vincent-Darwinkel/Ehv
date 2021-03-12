FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Event-Datepicker-Service/Event_Datepicker_Service.csproj", "Event-Datepicker-Service/"]

RUN dotnet restore "Event-Datepicker-Service/Event_Datepicker_Service.csproj"
COPY . .
WORKDIR "/src/Event-Datepicker-Service"
RUN dotnet build "Event_Datepicker_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Event_Datepicker_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Event_Datepicker_Service.dll"]