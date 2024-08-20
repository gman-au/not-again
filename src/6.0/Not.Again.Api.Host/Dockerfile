FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish

ARG PAT

COPY . /dotnet-app
RUN cd /dotnet-app && echo $(ls -1)
RUN \
    cd /dotnet-app && \
    dotnet nuget add source https://nuget.pkg.github.com/gman-au/index.json -n gman-au -u buildagent -p ${PAT} --store-password-in-clear-text && \
    dotnet restore "./Not.Again.Api.Host/Not.Again.Api.Host.csproj" && \
    dotnet publish "./Not.Again.Api.Host/Not.Again.Api.Host.csproj" --ignore-failed-sources --no-restore -c Release -o /app/publish /p:UseAppHost=false
    
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "Not.Again.Api.Host.dll"]