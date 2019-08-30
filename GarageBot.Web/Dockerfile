#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
#RUN "dotnet dev-certs https -t -v"
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview8 AS build
WORKDIR /src
COPY ["GarageBot.Web/GarageBot.Web.csproj", "GarageBot.Web/"]
COPY ["ChatServices/ChatServices.csproj", "ChatServices/"]
COPY ["Service.Twitch/Service.Twitch.csproj", "Service.Twitch/"]
COPY ["Service.Core/Service.Core.csproj", "Service.Core/"]
RUN dotnet restore "GarageBot.Web/GarageBot.Web.csproj"
COPY . .
WORKDIR "/src/GarageBot.Web"
RUN dotnet build "GarageBot.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "GarageBot.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GarageBot.Web.dll"]