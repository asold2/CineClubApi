FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CineClubApi/CineClubApi.csproj", "CineClubApi/"]
RUN dotnet restore "CineClubApi/CineClubApi.csproj"
COPY . .
WORKDIR "/src/CineClubApi"
RUN dotnet build "CineClubApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CineClubApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CineClubApi.dll"]
