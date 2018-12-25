FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Mizekar.MvcClients/Mizekar.MvcClients.csproj src/Mizekar.MvcClients/
RUN dotnet restore src/Mizekar.MvcClients/Mizekar.MvcClients.csproj
COPY . .
WORKDIR /src/src/Mizekar.MvcClients
RUN dotnet build Mizekar.MvcClients.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Mizekar.MvcClients.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mizekar.MvcClients.dll"]
