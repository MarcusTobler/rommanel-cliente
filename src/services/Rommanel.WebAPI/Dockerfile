FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/Rommanel.Cliente.WebAPI/Rommanel.Cliente.WebAPI.csproj", "src/services/Rommanel.Cliente.WebAPI/"]
RUN dotnet restore "src/services/Rommanel.Cliente.WebAPI/Rommanel.Cliente.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/services/Rommanel.Cliente.WebAPI"
RUN dotnet build "./Rommanel.Cliente.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Rommanel.Cliente.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rommanel.Cliente.WebAPI.dll"]
