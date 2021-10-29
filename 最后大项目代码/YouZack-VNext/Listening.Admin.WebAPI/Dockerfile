#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Listening.Admin.WebAPI/Listening.Admin.WebAPI.csproj", "Listening.Admin.WebAPI/"]
RUN dotnet restore "Listening.Admin.WebAPI/Listening.Admin.WebAPI.csproj"
COPY . .
WORKDIR "/src/Listening.Admin.WebAPI"
RUN dotnet build "Listening.Admin.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Listening.Admin.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Listening.Admin.WebAPI.dll"]