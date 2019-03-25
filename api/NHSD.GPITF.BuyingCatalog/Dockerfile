FROM microsoft/dotnet:2.1-sdk AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

COPY . .
RUN dotnet restore NHSD.GPITF.BuyingCatalog.sln

WORKDIR /src
RUN dotnet build --no-incremental NHSD.GPITF.BuyingCatalog.sln -c Release

WORKDIR /src/NHSD.GPITF.BuyingCatalog.Datastore.Database.Tests
RUN dotnet test --no-build -c Release

WORKDIR /src/NHSD.GPITF.BuyingCatalog.Datastore.CRM.Tests
RUN dotnet test --no-build -c Release

WORKDIR /src/NHSD.GPITF.BuyingCatalog.Logic.Tests
RUN dotnet test --no-build -c Release

WORKDIR /src/NHSD.GPITF.BuyingCatalog.Tests
RUN dotnet test --no-build -c Release

WORKDIR /src/NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint.Tests
RUN dotnet test --no-build -c Release

WORKDIR /src/NHSD.GPITF.BuyingCatalog.Search.Tests
RUN dotnet test --no-build -c Release

WORKDIR /src
RUN dotnet build --no-incremental NHSD.GPITF.BuyingCatalog.sln -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NHSD.GPITF.BuyingCatalog.dll"]