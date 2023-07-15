#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebUI/WebUI.csproj", "WebUI/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Security/Security.csproj", "Security/"]
RUN dotnet restore "WebUI/WebUI.csproj"
COPY . .
WORKDIR "/src/WebUI"

RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash - && apt-get install -yq nodejs build-essential
RUN npm install -g npm
RUN npm install

RUN dotnet build "WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebUI.dll"]