FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Mizekar.Accounts/Mizekar.Accounts.csproj", "src/Mizekar.Accounts/"]
RUN dotnet restore "src/Mizekar.Accounts/Mizekar.Accounts.csproj"
COPY . .
WORKDIR "/src/src/Mizekar.Accounts"
RUN dotnet build "Mizekar.Accounts.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Mizekar.Accounts.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mizekar.Accounts.dll"]