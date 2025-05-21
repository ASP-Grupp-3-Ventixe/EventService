# 1. Bygg-fasen
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EventApp/EventApp.csproj", "EventApp/"]
RUN dotnet restore "EventApp/EventApp.csproj"
COPY . .
WORKDIR "/src/EventApp"
RUN dotnet publish "EventApp.csproj" -c Release -o /app/publish

# 2. Runtime-fasen
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EventApp.dll"]
