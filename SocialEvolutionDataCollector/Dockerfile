FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 55835
ENV ASPNETCORE_URLS http://+:55835

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["SocialEvolutionDataCollector.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build "SocialEvolutionDataCollector.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SocialEvolutionDataCollector.csproj" -c Release -o /app


FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SocialEvolutionDataCollector.dll"]
