FROM mcr.microsoft.com/mssql/server
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestMagnetron/TestMagnetron.csproj", "TestMagnetron/"]
RUN dotnet restore "TestMagnetron/TestMagnetron.csproj"
COPY . .
WORKDIR "/src/TestMagnetron"
RUN dotnet build "TestMagnetron.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestMagnetron.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestMagnetron.dll"]