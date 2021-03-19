FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Datepicker_Service/Datepicker_Service.csproj", "Datepicker_Service/"]

RUN dotnet restore "Datepicker_Service/Datepicker_Service.csproj"
COPY . .
WORKDIR "/src/Datepicker_Service"
RUN dotnet build "Datepicker_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Datepicker_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Datepicker_Service.dll"]