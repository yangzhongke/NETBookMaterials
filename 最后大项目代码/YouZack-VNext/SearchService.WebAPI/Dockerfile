#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SearchService.WebAPI/SearchService.WebAPI.csproj", "SearchService.WebAPI/"]
RUN dotnet restore "SearchService.WebAPI/SearchService.WebAPI.csproj"
COPY . .
WORKDIR "/src/SearchService.WebAPI"
RUN dotnet build "SearchService.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SearchService.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SearchService.WebAPI.dll"]