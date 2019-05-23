FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 55836
ENV ASPNETCORE_URLS http://+:55836

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["CallStatisticsService.csproj", "./"]
RUN dotnet restore 
COPY . .
RUN dotnet build "CallStatisticsService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CallStatisticsService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CallStatisticsService.dll"]
