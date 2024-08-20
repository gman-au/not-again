FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Not.Again.Api.Host/Not.Again.Api.Host.csproj", "Not.Again.Api.Host/"]
RUN dotnet restore "Not.Again.Api.Host/Not.Again.Api.Host.csproj"
COPY . .
WORKDIR "/src/Not.Again.Api.Host"
RUN dotnet build "Not.Again.Api.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Not.Again.Api.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Not.Again.Api.Host.dll"]
