#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MediaEncoder.WebAPI/MediaEncoder.WebAPI.csproj", "MediaEncoder.WebAPI/"]
RUN dotnet restore "MediaEncoder.WebAPI/MediaEncoder.WebAPI.csproj"
COPY . .
WORKDIR "/src/MediaEncoder.WebAPI"
RUN dotnet build "MediaEncoder.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MediaEncoder.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MediaEncoder.WebAPI.dll"]