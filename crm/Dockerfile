FROM microsoft/dotnet:2.1-sdk

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

WORKDIR /app/IO.Swagger
COPY ./src/Gif.Service /app/IO.Swagger
COPY ./lib/Samc4.CipherUtil.dll /lib/Samc4.CipherUtil.dll

RUN dotnet publish -c Release -o /app

WORKDIR /app

EXPOSE 5001/tcp

ENTRYPOINT ["dotnet", "Gif.Service.dll"]
