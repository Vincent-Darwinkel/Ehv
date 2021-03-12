#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["File_Service/File_Service.csproj", "File_Service/"]
RUN dotnet restore "File_Service/File_Service.csproj"
COPY . .
WORKDIR "/src/File_Service"
RUN dotnet build "File_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "File_Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "File_Service.dll"]