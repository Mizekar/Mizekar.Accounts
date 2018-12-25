FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Mizekar.Api/Mizekar.Api.csproj src/Mizekar.Api/
RUN dotnet restore src/Mizekar.Api/Mizekar.Api.csproj
COPY . .
WORKDIR /src/src/Mizekar.Api
RUN dotnet build Mizekar.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Mizekar.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mizekar.Api.dll"]
