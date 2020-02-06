FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine3.10 AS build

WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -r linux-musl-x64 -c Release -o publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine3.10 AS publish

LABEL "com.datadoghq.ad.check_names"='["openmetrics"]'
LABEL "com.datadoghq.ad.init_configs"='[{}]'
LABEL "com.datadoghq.ad.instances"='[{"prometheus_url":"http://%%host%%:80/ops/metrics","namespace":"_","metrics":["mcg_webservice_api*"]}]'

ENV APP_ASPNETCORE_ENVIRONMENT=Development \
	APP_LOG_LEVEL=verbose \
	APP_CORS_ALLOWED_URLS=* \
	DD_TRACE_ANALYTICS_ENABLED=true \
	DD_AGENT_HOST=datadog-agent \
	DD_TRACE_AGENT_PORT=8126

WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Mcg.Webservice.Api.dll"] 