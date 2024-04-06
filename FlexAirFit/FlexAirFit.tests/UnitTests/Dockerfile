FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FlexAirFit.tests/FlexAirFit.Tests.csproj", "FlexAirFit.tests/"]
COPY ["FlexAirFit.Aplication/FlexAirFit.Application.csproj", "FlexAirFit.Aplication/"]
COPY ["FlexAirFit.Core/FlexAirFit.Core.csproj", "FlexAirFit.Core/"]
COPY ["FlexAirFit/FlexAirFit.csproj", "FlexAirFit/"]
RUN dotnet restore "FlexAirFit.tests/FlexAirFit.Tests.csproj"
COPY . .
WORKDIR "/src/FlexAirFit.tests"
RUN dotnet build "FlexAirFit.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FlexAirFit.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlexAirFit.Tests.dll"]
