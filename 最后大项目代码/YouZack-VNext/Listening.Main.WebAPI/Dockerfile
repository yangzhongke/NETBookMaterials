#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Listening.Main.WebAPI/Listening.Main.WebAPI.csproj", "Listening.Main.WebAPI/"]
RUN dotnet restore "Listening.Main.WebAPI/Listening.Main.WebAPI.csproj"
COPY . .
WORKDIR "/src/Listening.Main.WebAPI"
RUN dotnet build "Listening.Main.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Listening.Main.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Listening.Main.WebAPI.dll"]