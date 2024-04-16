FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["main-service.csproj", "./"]
RUN dotnet restore "main-service.csproj"
COPY . .
RUN dotnet tool install --global dotnet-ef --version 
RUN /root/.dotnet/tools/dotnet-ef migrations add InitialCreate --project main-service.csproj -v
RUN /root/.dotnet/tools/dotnet-ef database update

WORKDIR "/src/"
RUN dotnet build "main-service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "main-service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "main-service.dll"]